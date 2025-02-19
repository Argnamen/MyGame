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
            SpawnWorld(world[i], world[0]);
        }

        SpawnEnvironment(10);

        //SpawnExit();

        Debug.Log("Stage created successfully.");
    }

    private void SpawnWorld(Vector3 pos, Vector3 size)
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject plane = GameObject.Instantiate((GameObject)Resources.Load("Prefab/Plane"));

            switch (i)
            {
                case 0:
                    plane.transform.position = Vector3.zero;
                    break;
                case 1:
                    plane.transform.position = new Vector3(0, pos.y * 4, 0);
                    break;
                case 2:
                    plane.transform.position = new Vector3(pos.x * 4, pos.y * 4, 0);
                    break;
                case 3:
                    plane.transform.position = new Vector3(pos.x * 4, 0, 0);
                    break;
                case 4:
                    plane.transform.position = new Vector3(pos.x * 4, -pos.y * 4, 0);
                    break;
                case 5:
                    plane.transform.position = new Vector3(0, -pos.y * 4, 0);
                    break;
                case 6:
                    plane.transform.position = new Vector3(-pos.x * 4, -pos.y * 4, 0);
                    break;
                case 7:
                    plane.transform.position = new Vector3(-pos.x * 4, 0, 0);
                    break;
                case 8:
                    plane.transform.position = new Vector3(-pos.x * 4, pos.y * 4, 0);
                    break;
            }

            plane.GetComponent<SpriteRenderer>().size = (Vector2)size;
        }
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
            rockCharacter.Inventory.AddItem(new Item(ItemType.Resources, "Stone", Random.Range(1, 10)));

            SpawnClone(environmentGameObject);

            //_player.Character.EnemyList.Add(environmentComponent.Character);
        }

        _player.Character.SetEnvironments(_environments);
    }

    //test idea
    private void SpawnClone(GameObject origObject)
    {
        Vector3 world = _staticDataService.GetWorld(_staticDataService.CurrentRoom)[0];

        Transform origTransform = origObject.transform;

        GameObject obj = GameObject.Instantiate(origObject, origTransform);

        int worldSize = 4;

        for (int g = 0; g < 8; g++)
        {
            obj.name = origObject.name;
            if(g != 0)
                obj = GameObject.Instantiate(obj, origTransform);

            MonoBehaviour.Destroy(obj.GetComponent<Environment>());

            switch (g)
            {
                case 0:
                    obj.transform.localPosition = new Vector3(
                        0,
                        0 + world.y * worldSize,
                        0);
                    break;
                case 1:
                    obj.transform.localPosition = new Vector3(
                        0 + world.x * worldSize,
                        0 + world.y * worldSize,
                        0);
                    break;
                case 2:
                    obj.transform.localPosition = new Vector3(
                        0 + world.x * worldSize,
                        0,
                        0);
                    break;
                case 3:
                    obj.transform.localPosition = new Vector3(
                        0 + world.x * worldSize,
                        0 - world.y * worldSize,
                        0);
                    break;
                case 4:
                    obj.transform.localPosition = new Vector3(
                        0,
                        0 - world.y * worldSize,
                        0);
                    break;
                case 5:
                    obj.transform.localPosition = new Vector3(
                        0 - world.x * worldSize,
                        0 - world.y * worldSize,
                        0);
                    break;
                case 6:
                    obj.transform.localPosition = new Vector3(
                        0 - world.x * worldSize,
                        0,
                        0);
                    break;
                case 7:
                    obj.transform.localPosition = new Vector3(
                        0 - world.x * worldSize,
                        0 + world.y * worldSize,
                        0);
                    break;
            }
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




