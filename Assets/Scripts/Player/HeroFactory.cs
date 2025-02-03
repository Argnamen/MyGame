using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class HeroFactory : IHeroFactory
{
    private readonly DiContainer _diContainer;
    private List<Minion> _minions;
    private List<Environment> _environments;
    private Player _player;
    private IStaticDataService _staticDataService;
    private ItemsInWorld _itemsInWorld;
    private CameraService _cameraService;

    public HeroFactory(DiContainer diContainer, List<Minion> minions, List<Environment> environments, IStaticDataService staticDataService, ItemsInWorld itemsInWorld, CameraService cameraService)
    {
        _diContainer = diContainer;
        _minions = minions;
        _environments = environments;
        _staticDataService = staticDataService;
        _itemsInWorld = itemsInWorld;
        _cameraService = cameraService;
    }

    public Player CreateHero()
    {
        if (_player != null)
        {
            return _player;
        }

        var playerPrefab = _diContainer.Resolve<Player>();

        Bow weapon = new Bow(_diContainer, _staticDataService, _itemsInWorld, TypeWeapon.Range, "Bow", 10, 2f, 5f, 90);
        _player = _diContainer.InstantiatePrefabForComponent<Player>(playerPrefab);

        Debug.Log("Player instance created.");

        PlayerCharacter playerCharacter = new PlayerCharacter(100, 0.1f, 1f, weapon, Vector2.zero, _environments.ToArray(), _staticDataService, _itemsInWorld);
        playerCharacter.Inventory = new Inventory();
        playerCharacter.Inventory.AddItem(new Item(weapon));
        _player.Character = playerCharacter;

        Debug.Log("PlayerCharacter instance created and assigned to Player.");

        _diContainer.Inject(playerCharacter);
        _diContainer.Inject(_player.Character.Weapon);
        _diContainer.Inject(_player);

        var playerInput = ProjectContext.Instance.Container.Resolve<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput is null in ProjectContext!");
            return null;
        }

        var virtualCamera = ProjectContext.Instance.Container.Resolve<CinemachineVirtualCamera>();
        var playerTransform = _player.transform;
        var inputService = new InputService(playerInput, _player, playerTransform, _cameraService);

        _player.SetInputService(inputService);
        SetupVirtualCamera(virtualCamera, playerTransform);

        Debug.Log("Hero created successfully.");
        return _player;
    }

    public Player GetHero()
    {
        return _player ?? CreateHero();
    }

    private void SetupVirtualCamera(CinemachineVirtualCamera virtualCamera, Transform playerTransform)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerTransform;
            virtualCamera.LookAt = playerTransform;
        }
    }
}

public interface IHeroFactory
{
    Player CreateHero();
    Player GetHero();
}





