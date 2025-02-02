using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StageFactory : IStageFactory
{
    private readonly IStaticDataService _staticDataService;
    private List<Environment> _environments;
    private Player _player;
    private readonly DiContainer _diContainer;
    private readonly IHeroFactory _heroFactory;

    public StageFactory(IStaticDataService staticDataService, DiContainer diContainer, List<Environment> environments, IHeroFactory heroFactory)
    {
        _staticDataService = staticDataService;
        _diContainer = diContainer;
        _environments = environments;
        _heroFactory = heroFactory;
    }

    public void CreateStage()
    {
        _player = _heroFactory.GetHero();
        if (_player.Character == null)
        {
            Debug.LogError("PlayerCharacter is null after hero creation.");
            return;
        }

        Debug.Log("Player created successfully.");

        List<Vector3> world = _staticDataService.GetWorld(1);

        for (int i = 0; i < world.Count; i++)
        {
            SpawnWorld(world[i], i);
        }

        SpawnEnvironment(10);

        Debug.Log("Stage created successfully.");
    }

    private void SpawnWorld(Vector3 size, int num)
    {
        GameObject plane = GameObject.Instantiate((GameObject)Resources.Load("Prefab/Plane"));
        plane.transform.position = Vector3.forward * size.z;
        plane.GetComponent<SpriteRenderer>().size = (Vector2)size;
    }

    private void SpawnEnvironment(int count)
    {
        List<Vector3> world = _staticDataService.GetWorld(1);

        for (int i = 0; i < count; i++)
        {
            GameObject environmentGameObject = GameObject.Instantiate((GameObject)Resources.Load("Prefab/Rock"));
            Environment environmentComponent = environmentGameObject.GetComponent<Environment>();
            _environments.Add(environmentComponent);

            int currentRoomIndex = _staticDataService.CurrentRoom;
            if (currentRoomIndex < 0 || currentRoomIndex >= world.Count)
            {
                Debug.LogError("CurrentRoom index is out of range.");
                return;
            }

            Vector3 roomSize = world[currentRoomIndex];

            environmentGameObject.transform.position = new Vector3(
                Random.Range(-(roomSize.x * 2), roomSize.x * 2),
                Random.Range(-(roomSize.y * 2), roomSize.y * 2),
                0);

            var itemsInWorld = _diContainer.Resolve<ItemsInWorld>();
            if (itemsInWorld == null)
            {
                Debug.LogError("ItemsInWorld is null.");
                return;
            }

            var playerCharacter = _player.Character;
            if (playerCharacter == null)
            {
                Debug.LogError("PlayerCharacter is null when creating Rock.");
                return;
            }

            var rockCharacter = new Rock(100, 0.1f, 1f, null, environmentGameObject.transform.position, playerCharacter, _environments.ToArray(), _staticDataService, itemsInWorld);
            environmentComponent.Initialize(rockCharacter);

            rockCharacter.Inventory = new Inventory();
            rockCharacter.Inventory.AddItem(new Item(ItemType.Resources, "Stone"));

            _player.Character.EnemyList.Add(environmentComponent.Character);
        }
    }
}

public interface IStageFactory
{
    void CreateStage();
}




