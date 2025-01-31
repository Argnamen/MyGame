using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public abstract class Character
{
    private WorldsMap _worldsMap;
    private ItemsInWorld _itemsInWorld;

    protected int _healt;
    protected float _spead;
    protected float _size;

    public Weapon Weapon;

    protected Vector2 _startPos;

    protected Environment[] _environments;

    public int HP => _healt;
    public float Size => _size;

    public Vector2 Position => _startPos;

    public Environment[] Environments => _environments;


    public UnityEvent DamageEvent = new UnityEvent();
    public UnityEvent DeathEvent = new UnityEvent();

    public Inventory Inventory;

    private bool _isDamageUpTime = false;

    [Inject]
    private DiContainer _diContainer;

    [Inject]
    public void Constuct(WorldsMap worldsMap, ItemsInWorld itemsInWorld)
    {
        _worldsMap = worldsMap;
        _itemsInWorld = itemsInWorld;
    }

    public Character(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Environment[] environments)
    {
        _healt = healt;
        _spead = spead;
        _startPos = startPos;
        _environments = environments;

        _size = size;

        Weapon = weapon;

        if(Inventory == null)
            Inventory = new Inventory();
    }
    public virtual Vector2 Move(Direction direction)
    {
        Vector2 newPos = UpdatePos(direction);

        int world = _worldsMap.CorrectWorld;

        if ( _worldsMap.GetWorld(world)[0].x * 2 < Mathf.Abs(newPos.x) || _worldsMap.GetWorld(world)[0].y * 2 < Mathf.Abs(newPos.y))
            return _startPos;

        foreach (var env in _environments)
        {
            if (env != null && (Vector2.Distance(newPos, env.transform.position) < env.Character._size))
                return _startPos;
        }

        _startPos = newPos;

        return newPos;
    }

    public virtual void Attack(Character enemy)
    {
        enemy.UpdateHP(-Weapon.Damage);
    }

    public virtual void UpdateHP(int hp)
    {
        if (hp < 0)
        {
            DamageEvent?.Invoke();
        }
        _healt += hp;

        if(_healt <= 0)
        {
            Item[] items = Inventory.GetAllItems();

            DropItems(Inventory.GetAllItems());

            DeathEvent?.Invoke();
        }
    }

    public void DropItems(Item[] items)
    {
        Item drop = null;

        GameObject dropObject = null;

        if (items == null || items.Length == 0)
            return;

        foreach (var item in items) 
        {
            drop = Inventory.DropItem(item);

            if(drop != null)
            {
                dropObject = _diContainer.InstantiatePrefab((GameObject)Resources.Load($"Prefab/{drop.ID}"));

                dropObject.transform.position = Position + Vector2.one * Random.value;

                dropObject.GetComponent<ObjectInWorld>().Item = drop;

                _itemsInWorld.PointsItems.Add(dropObject.transform.position, dropObject.GetComponent<ObjectInWorld>());
            }
        }
    }

    private Vector2 UpdatePos(Direction direction)
    {
        Vector2 newPos = _startPos;

        switch (direction)
        {
            case Direction.Up:
                newPos += Vector2.up * _spead;
                break;
            case Direction.Down:
                newPos += Vector2.down * _spead;
                break;
            case Direction.Left:
                newPos += Vector2.left * _spead;
                break;
            case Direction.Right:
                newPos += Vector2.right * _spead;
                break;
            default:
                newPos += Vector2.zero * _spead;
                break;
        }

        return newPos;
    }

    public virtual IEnumerator Damage(Character[] evilCharacters, bool isAuto)
    {
        if (_isDamageUpTime)
            yield break;

        _isDamageUpTime = true;

        while (isAuto)
        {
            int c = 0;
            Character centralEnemy = null;
            float minDistance = float.MaxValue;
            float lowestHP = float.MaxValue;

            for (int i = 0; i < evilCharacters.Length; i++)
            {
                if (this != null && evilCharacters[i].HP > 0)
                {
                    float distance = Vector2.Distance(Position, evilCharacters[i].Position) - evilCharacters[i].Size;

                    if (distance <= Weapon.Radius)
                    {
                        if (centralEnemy == null || (distance < minDistance && minDistance > Weapon.Radius))
                        {
                            minDistance = distance;
                            centralEnemy = evilCharacters[i];
                        }
                        else if(centralEnemy == null || (minDistance <= Weapon.Radius && evilCharacters[i].HP < lowestHP))
                        {
                            centralEnemy = evilCharacters[i];
                            lowestHP = evilCharacters[i].HP;
                        }
                    }
                }
                else if (this == null)
                    yield break;
            }

            if (centralEnemy != null)
            {
                Vector2 centralPoint = centralEnemy.Position;

                for (int i = 0; i < evilCharacters.Length; i++)
                {
                    if (this != null && evilCharacters[i].HP > 0)
                    {
                        float distance = Vector2.Distance(Position, evilCharacters[i].Position) - evilCharacters[i].Size;

                        if (distance <= Weapon.Radius)
                        {
                            Vector2 directionToEnemy = (evilCharacters[i].Position - Position).normalized;
                            Vector2 directionToCentral = (centralPoint - Position).normalized;
                            float angleToEnemy = Vector2.SignedAngle(directionToCentral, directionToEnemy);

                            if (Mathf.Abs(angleToEnemy) <= Weapon.Corner / 2)
                            {
                                c++;

                                if (Weapon.CreateProjectile(this, Weapon, centralEnemy, evilCharacters))
                                    break;

                                Attack(evilCharacters[i]);
                            }
                        }
                    }
                    else if (this == null)
                        yield break;
                }
            }

            if (c > 0)
            {
                yield return new WaitForSeconds(Weapon.Uptime);
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }

        _isDamageUpTime = false;
    }

    public enum Direction
    {
        None, Up, Down, Left, Right
    }
}
