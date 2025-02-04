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
        List<Vector3> world = _staticDataService.GetWorld(_staticDataService.CurrentRoom);

        _player = _heroFactory.GetHero();

        for (int i = 0; i < world.Count; i++)
        {
            SpawnWorld(world[i], i);
        }

        SpawnEnvironment(10);

        SpawnExit();

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
        List<Vector3> world = _staticDataService.GetWorld(_staticDataService.CurrentRoom);

        for (int i = 0; i < count; i++)
        {
            GameObject environmentGameObject = GameObject.Instantiate((GameObject)Resources.Load("Prefab/Rock"));
            Environment environmentComponent = environmentGameObject.GetComponent<Environment>();
            _environments.Add(environmentComponent);

            int currentRoomIndex = _staticDataService.CurrentRoom;

            Vector3 roomSize = world[currentRoomIndex];

            environmentGameObject.transform.position = new Vector3(
                Random.Range(-(roomSize.x * 2), roomSize.x * 2),
                Random.Range(-(roomSize.y * 2), roomSize.y * 2),
                roomSize.z);

            var itemsInWorld = _diContainer.Resolve<ItemsInWorld>();

            var playerCharacter = _player.Character;

            var rockCharacter = new Rock(100, 0.1f, 1f, null, environmentGameObject.transform.position, playerCharacter, _environments.ToArray(), _staticDataService, itemsInWorld);
            environmentComponent.Initialize(rockCharacter);

            rockCharacter.Inventory = new Inventory();
            rockCharacter.Inventory.AddItem(new Item(ItemType.Resources, "Stone"));

            //_player.Character.EnemyList.Add(environmentComponent.Character);
        }
    }

    private void SpawnExit()
    {
        GameObject environmentGameObject = GameObject.Instantiate((GameObject)Resources.Load("Prefab/Exit"));
        List<Vector3> world = _staticDataService.GetWorld(_staticDataService.CurrentRoom);

        int currentRoomIndex = _staticDataService.CurrentRoom;

        Vector3 roomSize = world[currentRoomIndex];

        environmentGameObject.transform.position = new Vector3(0, roomSize.y * 2, roomSize.z);
    }
}

public interface IStageFactory
{
    void CreateStage();
}




