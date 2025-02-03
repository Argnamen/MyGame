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

    public EnemyFactory(DiContainer diContainer, List<Minion> minions, IStaticDataService staticDataService, ItemsInWorld itemsInWorld, IHeroFactory heroFactory)
    {
        _diContainer = diContainer;
        _minions = minions;
        _staticDataService = staticDataService;
        _itemsInWorld = itemsInWorld;
        _heroFactory = heroFactory;
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
                Random.Range(-(world[_staticDataService.CurrentRoom].y * 2), world[_staticDataService.CurrentRoom].y * 2),
                0);

            if (_player.Character == null)
            {
                Debug.LogError("PlayerCharacter is null when creating Enemy.");
                continue;
            }

            var enemyCharacter = new Enemy(100, 0.01f, 1f, weapon, gameObject.transform.position, _player, _minions.ToArray(), i, _diContainer.Resolve<Environment[]>(), _staticDataService, _itemsInWorld);

            _minions[i].Character = enemyCharacter;

            _minions[i].Character.Inventory = new Inventory();
            _minions[i].Character.Inventory.AddItem(new Item(ItemType.Resources, "Stone"));
            _minions[i].Character.Inventory.AddItem(new Item(weapon));

            // Внедряем зависимости вручную
            _diContainer.Inject(_minions[i].Character);
        }

        Debug.Log("Enemies created successfully.");
    }
}

public interface IEnemyFactory
{
    void CreateEnemies(int count);
}





