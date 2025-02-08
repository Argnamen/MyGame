using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyFactory : IEnemyFactory
{
    private readonly DiContainer _diContainer;
    private List<Minion> _minions;
    private Player _player;
    private IStaticDataService _staticDataService;
    private ItemsInWorld _itemsInWorld;
    private IHeroFactory _heroFactory;
    private ISetupCamera _setupCamera;

    public EnemyFactory(DiContainer diContainer, List<Minion> minions, IStaticDataService staticDataService, ItemsInWorld itemsInWorld, IHeroFactory heroFactory, ISetupCamera setupCamera)
    {
        _diContainer = diContainer;
        _minions = minions;
        _staticDataService = staticDataService;
        _itemsInWorld = itemsInWorld;
        _heroFactory = heroFactory;
        _setupCamera = setupCamera;
    }

    public void CreateEnemies(int count)
    {
        List<Vector3> world = _staticDataService.GetWorld(_staticDataService.CurrentRoom);
        GameObject gameObject = null;
        Weapon weapon = null;

        _player = _heroFactory.GetHero();

        for (int i = 0; i < count; i++)
        {
            gameObject = GameObject.Instantiate((GameObject)Resources.Load("Prefab/Enemy"));
            weapon = new Sword(TypeWeapon.Melee, "Sword", 1, 2f, 5f, 90);
            _minions.Add(gameObject.GetComponent<Minion>());

            gameObject.transform.position = new Vector3(
                Random.Range(-(world[_staticDataService.CurrentRoom].x * 2), world[_staticDataService.CurrentRoom].x * 2),
                Random.Range(-(world[_staticDataService.CurrentRoom].y * 1.2f), world[_staticDataService.CurrentRoom].y * 2),
                0);

            var enemyCharacter = new Enemy(100, 1f, 10f, 1f, weapon, gameObject.transform.position, _player, _minions.ToArray(), i, _diContainer.Resolve<Environment[]>(), _staticDataService, _itemsInWorld);

            _minions[i].Character = enemyCharacter;

            _minions[i].Character.Inventory = new Inventory();
            _minions[i].Character.Inventory.AddItem(new Item(ItemType.Resources, "Stone"));
            _minions[i].Character.Inventory.AddItem(new Item(weapon));

            _player.Character.EnemyList.Add(enemyCharacter);

            // Внедряем зависимости вручную
            _diContainer.Inject(_minions[i].Character);

            _diContainer.Inject(_minions[i]);

            _setupCamera.SetupVirtualCamera(gameObject.transform, false);
        }

        Debug.Log("Enemies created successfully.");
    }
}

public interface IEnemyFactory
{
    void CreateEnemies(int count);
}





