using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Spawner: MonoBehaviour
{
    private MinionList _enemys;
    private PlayerModels _playerModels;
    private WorldsMap _worldsMap;

    [Inject]
    private DiContainer _diContainer;

    private Player _player;
    private List<Minion> _minions = new List<Minion>();
    private List<Environment> _environments = new List<Environment>();

    [Inject]
    public void Construct(MinionList enemys, PlayerModels playerModels, WorldsMap worldsMap)
    {
        _enemys = enemys;
        _playerModels = playerModels;
        _worldsMap = worldsMap;
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (_playerModels == null || _enemys == null)
            yield return new WaitForSeconds(1);

        List<Vector3> world = _worldsMap.GetWorld(1);

        for (int i = 0; i < world.Count; i++)
        {
            SpawnWorld(world[i], i);
        }

        SpawnEnvironment(10);

        SpawnPlayer();

        SpawnMinions(0);

        UpdateSource();

        for (int i = 0; i < _minions.Count; i++)
        {
            _player.EnemyList.Add(_minions[i].Character);
        }

        for (int i = 0; i < _environments.Count; i++)
        {
            _player.EnemyList.Add(_environments[i].Character);
        }
    }

    public void SpawnWorld(Vector3 size, int num)
    {
        GameObject plane = Instantiate<GameObject>((GameObject)Resources.Load("Prefab/Plane"));

        plane.transform.position = Vector3.forward * size.z;

        plane.GetComponent<SpriteRenderer>().size = (Vector2)size;
    }

    public void SpawnMinions(int enemyCount)
    {
        List<Vector3> world = _worldsMap.GetWorld(1);

        for (int i = 0; i < enemyCount; i++)
        {
            _minions.Add(Instantiate<GameObject>((GameObject)Resources.Load("Prefab/Enemy")).GetComponent<Minion>());

            _minions[i].gameObject.transform.position = new Vector3(
                Random.Range(-(world[_worldsMap.CurrentRoom].x * 2), world[_worldsMap.CurrentRoom].x * 2),
                Random.Range(-(world[_worldsMap.CurrentRoom].y * 2), world[_worldsMap.CurrentRoom].y * 2),
                0);
        }
    }

    public void SpawnEnvironment(int enemyCount)
    {
        List<Vector3> world = _worldsMap.GetWorld(1);

        for (int i = 0; i < enemyCount; i++)
        {
            _environments.Add(Instantiate<GameObject>((GameObject)Resources.Load("Prefab/Rock")).GetComponent<Environment>());

            _environments[i].gameObject.transform.position = new Vector3(
                Random.Range(-(world[_worldsMap.CurrentRoom].x * 2), world[_worldsMap.CurrentRoom].x * 2),
                Random.Range(-(world[_worldsMap.CurrentRoom].y * 2), world[_worldsMap.CurrentRoom].y * 2),
                0);
        }
    }

    public void SpawnPlayer()
    {
        _player = _diContainer.InstantiatePrefab((GameObject)Resources.Load("Prefab/Player")).GetComponent<Player>();        
    }

    private void UpdateSource()
    {
        for (int i = 0; i < _environments.Count; i++)
        {
            _environments[i].Character = new Rock(100, 0.1f, 1f, null, _environments[i].gameObject.transform.position, _player, _minions.ToArray(), i, _environments.ToArray());

            _environments[i].Character.Inventory = new Inventory();

            _diContainer.Inject(_environments[i].Character);
        }

        for (int i = 0; i < _minions.Count; i++)
        {
            _minions[i].Character = new TestEnemy(100, 0.01f, 1f, new Sword(TypeWeapon.Melee, "1", 0, 1f, 5f, 90), _minions[i].gameObject.transform.position, _player, _minions.ToArray(), i, _environments.ToArray());

            _minions[i].Character.Inventory = new Inventory();

            _diContainer.Inject(_minions[i].Character);
        }

        Weapon playerWeapon = new Bow(TypeWeapon.Range, "1", 10, 2f, 5f, 90);

        _player.Character = new TestCharacter(100, 0.1f, 1f, playerWeapon, Vector2.zero, _environments.ToArray());

        _player.Character.Inventory = new Inventory();

        _diContainer.Inject(_player.Character);

        _diContainer.Inject(_player.Character.Weapon);
    }
}
