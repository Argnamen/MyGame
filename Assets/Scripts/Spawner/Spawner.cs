using Cinemachine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;
using System.Collections;

public class Spawner : MonoBehaviour
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

        GameObject gameObject = null;

        Weapon weapon = null;

        for (int i = 0; i < enemyCount; i++)
        {
            gameObject = Instantiate<GameObject>((GameObject)Resources.Load("Prefab/Enemy"));

            weapon = new Sword(TypeWeapon.Melee, "Sword", 1, 2f, 5f, 90);

            _minions.Add(gameObject.GetComponent<Minion>());

            gameObject.transform.position = new Vector3(
                Random.Range(-(world[_worldsMap.CurrentRoom].x * 2), world[_worldsMap.CurrentRoom].x * 2),
                Random.Range(-(world[_worldsMap.CurrentRoom].y * 2), world[_worldsMap.CurrentRoom].y * 2),
                0);

            _minions[i].Character = new TestEnemy(100, 0.01f, 1f, weapon, _minions[i].gameObject.transform.position, _player, _minions.ToArray(), i, _environments.ToArray());

            _minions[i].Character.Inventory = new Inventory();

            _minions[i].Character.Inventory.AddItem(new Item(ItemType.Resources, "Stone"));

            _minions[i].Character.Inventory.AddItem(new Item(weapon));

            _diContainer.Inject(_minions[i].Character);
        }
    }

    public void SpawnEnvironment(int enemyCount)
    {
        List<Vector3> world = _worldsMap.GetWorld(1);

        GameObject gameObject = null;

        for (int i = 0; i < enemyCount; i++)
        {
            gameObject = Instantiate<GameObject>((GameObject)Resources.Load("Prefab/Rock"));

            _environments.Add(gameObject.GetComponent<Environment>());

            gameObject.transform.position = new Vector3(
                Random.Range(-(world[_worldsMap.CurrentRoom].x * 2), world[_worldsMap.CurrentRoom].x * 2),
                Random.Range(-(world[_worldsMap.CurrentRoom].y * 2), world[_worldsMap.CurrentRoom].y * 2),
                0);

            _environments[i].Character = new Rock(100, 0.1f, 1f, null, _environments[i].gameObject.transform.position, _player, _minions.ToArray(), i, _environments.ToArray());

            _environments[i].Character.Inventory = new Inventory();

            _environments[i].Character.Inventory.AddItem(new Item(ItemType.Resources, "Stone"));

            _diContainer.Inject(_environments[i].Character);
        }
    }

    public void SpawnPlayer()
    {
        GameObject gameObject = (GameObject)Resources.Load("Prefab/Player");

        Weapon weapon = new Bow(TypeWeapon.Range, "Bow", 10, 2f, 5f, 90);

        _player = _diContainer.InstantiatePrefab(gameObject).GetComponent<Player>();

        TestCharacter playerCharacter = new TestCharacter(100, 0.1f, 1f, weapon, Vector2.zero, _environments.ToArray());

        playerCharacter.Inventory = new Inventory();
        playerCharacter.Inventory.AddItem(new Item(weapon));

        _player.Character = playerCharacter;

        _diContainer.Inject(playerCharacter);
        _diContainer.Inject(_player.Character.Weapon);

        _diContainer.Inject(_player);

        var playerInput = ProjectContext.Instance.Container.Resolve<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput is null in ProjectContext!");
            return;
        }

        //var inputProvider = ProjectContext.Instance.Container.Resolve<CinemachineInputProvider>();

        var virtualCamera = ProjectContext.Instance.Container.Resolve<CinemachineVirtualCamera>();

        var playerTransform = _player.transform;

        var inputService = new InputService(playerInput, playerCharacter, playerTransform, virtualCamera);

        _player.SetInputService(inputService);

        SetupVirtualCamera(virtualCamera, playerTransform);
    }

    public void SetupVirtualCamera(CinemachineVirtualCamera virtualCamera, Transform playerTransform)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerTransform;
            virtualCamera.LookAt = playerTransform;
        }
    }






}

