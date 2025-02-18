using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public abstract class Character
{
    protected IStaticDataService _staticDataService;
    private ItemsInWorld _itemsInWorld;
    protected IGameMode _gameMode;
    private ICameraService _cameraService;

    protected int _healt;
    protected float _spead;
    protected float _size;

    protected float _defautSpead;

    public Weapon Weapon;
    public Weapon Tool;

    protected Vector2 _startPos;

    protected Environment[] _environments;

    public int HP => _healt;
    public float Size => _size;
    public Vector2 Position => _startPos;
    public Environment[] Environments => _environments;

    public float Spead => _spead;

    public UnityEvent DamageEvent = new UnityEvent();
    public UnityEvent DeathEvent = new UnityEvent();
    public Inventory Inventory;

    protected bool _isStealsModOn = false;

    public bool IsPeace = true;

    [Inject]
    public void Construct(IStaticDataService staticDataService, ItemsInWorld itemsInWorld, IGameMode gameMode, ICameraService cameraService)
    {
        _staticDataService = staticDataService;
        _itemsInWorld = itemsInWorld;
        _gameMode = gameMode;
        _cameraService = cameraService;

        _gameMode.StealthMod += OnStealsMod;
        _gameMode.ReturnMod += ReturnMode;
    }

    public Character(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Environment[] environments)
    {
        _healt = healt;
        _spead = spead;
        _startPos = startPos;
        _environments = environments;

        _defautSpead = spead;

        _size = size;
        Weapon = weapon;

        if (Tool == null)
            Tool = weapon;
    }

    public Character(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Environment[] environments, IStaticDataService staticDataService, ItemsInWorld itemsInWorld)
    {
        _healt = healt;
        _spead = spead;
        _startPos = startPos;
        _environments = environments;
        _staticDataService = staticDataService;
        _itemsInWorld = itemsInWorld;

        _defautSpead = spead;

        _size = size;
        Weapon = weapon;

        if (Tool == null)
            Tool = weapon;
    }

    public virtual void OnStealsMod()
    {
        _isStealsModOn = true;

        _spead = _defautSpead / 2;
    }

    public virtual void ReturnMode() 
    {
        _isStealsModOn = false;

        _spead = _defautSpead;
    }

    public virtual Vector2 Move(Direction direction)
    {
        Vector2 newPos = UpdatePos(direction);

        int world = _staticDataService.CurrentRoom;

        if (_staticDataService.GetWorld(world)[0].x * 2 < Mathf.Abs(newPos.x) || _staticDataService.GetWorld(world)[0].y * 2 < Mathf.Abs(newPos.y))
        {
            if (_staticDataService.GetWorld(world)[0].x * 2 < Mathf.Abs(newPos.x))
            {
                _startPos *= new Vector2(-1, 1);
            }
            else
            {
                _startPos *= new Vector2(1, -1);
            }

            newPos = UpdatePos(direction);

            return newPos;
        }

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
        Weapon weapon = Weapon;

        if (IsPeace)
            weapon = Tool;
        else
            weapon = Weapon;

        enemy.UpdateHP(-weapon.Damage);

        Debug.Log(enemy.HP);
    }

    public virtual void UpdateHP(int hp)
    {
        if (hp < 0)
        {
            DamageEvent?.Invoke();
        }
        _healt += hp;

        if (_healt <= 0)
        {
            Item[] items = Inventory.GetAllItems();
            DropItems(items);
            DeathEvent?.Invoke();
        }
    }

    public void DropItems(Item[] items)
    {
        if (items == null || items.Length == 0)
            return;

        foreach (var item in items)
        {
            var drop = Inventory.DropItem(item);

            if (drop != null)
            {
                var dropObject = GameObject.Instantiate((GameObject)Resources.Load($"Prefab/{drop.ID}"));
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
                newPos = Vector2.MoveTowards(_startPos, newPos + Vector2.up * _spead, _spead);
                break;
            case Direction.Down:
                newPos = Vector2.MoveTowards(_startPos, newPos + Vector2.down * _spead, _spead);
                break;
            case Direction.Left:
                newPos = Vector2.MoveTowards(_startPos, newPos + Vector2.left * _spead, _spead);
                break;
            case Direction.Right:
                newPos = Vector2.MoveTowards(_startPos, newPos + Vector2.right * _spead, _spead);
                break;
            default:
                newPos = Vector2.MoveTowards(_startPos, newPos + Vector2.zero * _spead, _spead);
                break;
        }

        return newPos;
    }

    public virtual IEnumerator Damage(Character[] evilCharacters, bool isAuto)
    {
        Weapon weapon = Weapon;

        if (IsPeace)
            weapon = Tool;
        else
            weapon = Weapon;

        if (weapon.IsUptime)
            yield break;
        
        do
        {
            if (weapon.IsUptime && isAuto)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }

            if (isAuto)
            {
                if (IsPeace && _isStealsModOn)
                {
                    yield return new WaitForFixedUpdate();
                    continue;
                }
            }

            int c = 0;
            Character centralEnemy = null;
            float minDistance = float.MaxValue;
            float lowestHP = float.MaxValue;

            for (int i = 0; i < evilCharacters.Length; i++)
            {
                if (this != null && evilCharacters[i].HP > 0)
                {
                    float distance = Vector2.Distance(Position, evilCharacters[i].Position) - evilCharacters[i].Size;

                    if (distance <= weapon.Radius)
                    {
                        if (centralEnemy == null || (distance < minDistance && minDistance > weapon.Radius))
                        {
                            minDistance = distance;
                            centralEnemy = evilCharacters[i];
                        }
                        else if (centralEnemy == null || (minDistance <= weapon.Radius && evilCharacters[i].HP < lowestHP))
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

                        if (distance <= weapon.Radius)
                        {
                            Vector2 directionToEnemy = (evilCharacters[i].Position - Position).normalized;
                            Vector2 directionToCentral = (centralPoint - Position).normalized;
                            float angleToEnemy = Vector2.SignedAngle(directionToCentral, directionToEnemy);

                            if (Mathf.Abs(angleToEnemy) <= weapon.Corner / 2)
                            {
                                c++;

                                if (weapon.CreateProjectile(this, weapon, centralEnemy, evilCharacters))
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
                weapon.UpdateUptime();
            }

            yield return new WaitForFixedUpdate();
        }
        while (isAuto);
    }

    public enum Direction
    {
        None, Up, Down, Left, Right
    }
}

