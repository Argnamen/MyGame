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
    private ICameraService _cameraService;
    private ISetupCamera _setupCamera;

    public HeroFactory(DiContainer diContainer, List<Minion> minions, List<Environment> environments, IStaticDataService staticDataService, ItemsInWorld itemsInWorld, ICameraService cameraService, ISetupCamera setupCamera)
    {
        _diContainer = diContainer;
        _minions = minions;
        _environments = environments;
        _staticDataService = staticDataService;
        _itemsInWorld = itemsInWorld;
        _cameraService = cameraService;
        _setupCamera = setupCamera;
    }

    public Player CreateHero()
    {
        if (_player != null)
        {
            return _player;
        }

        var playerPrefab = _diContainer.Resolve<Player>();

        List<Vector3> world = _staticDataService.GetWorld(_staticDataService.CurrentRoom);

        Bow weapon = new Bow(_diContainer, _staticDataService, _itemsInWorld, TypeWeapon.Range, "Bow", 10, 50f, 5f, 90);
        _player = _diContainer.InstantiatePrefabForComponent<Player>(playerPrefab);

        _player.transform.position = new Vector3(0,-(world[_staticDataService.CurrentRoom].y * 2) + world[_staticDataService.CurrentRoom].y % 2, world[_staticDataService.CurrentRoom].z);

        Debug.Log("Player instance created.");

        PlayerCharacter playerCharacter = new PlayerCharacter(100, 0.1f, 1f, weapon, _player.transform.position, _environments.ToArray(), _staticDataService, _itemsInWorld);
        playerCharacter.Inventory = new Inventory();
        playerCharacter.Inventory.AddItem(new Item(weapon));
        _player.Character = playerCharacter;

        Debug.Log("PlayerCharacter instance created and assigned to Player.");

        _diContainer.Inject(playerCharacter);
        _diContainer.Inject(_player.Character.Weapon);
        _diContainer.Inject(_player);

        var playerInput = ProjectContext.Instance.Container.Resolve<PlayerInput>();

        _setupCamera.SetupVirtualCamera(_player.transform, true);

        _player.SetInputService(new InputService(playerInput, _player, _player.transform, _cameraService));

        Debug.Log("Hero created successfully.");
        return _player;
    }

    public Player GetHero()
    {
        return _player ?? CreateHero();
    }
}

public interface IHeroFactory
{
    Player CreateHero();
    Player GetHero();
}





