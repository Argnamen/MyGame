using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Rock : Enemy
{
    [Inject]
    private DiContainer _diContainer;

    private ItemsInWorld _itemsInWorld;
    [Inject]
    public void Construct(ItemsInWorld itemsInWorld)
    {
        _itemsInWorld = itemsInWorld;
    }
    public Rock(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Player player, Minion[] minion, int numberMinion, Environment[] environments) : base(healt, spead, size, weapon, startPos, player, minion, numberMinion, environments)
    {
        DeathEvent.AddListener(Death);
    }

    private void Death()
    {
        GameObject stone = _diContainer.InstantiatePrefab(Resources.Load("Prefab/Stone"));

        Item item = new Item(ItemType.Resources);

        stone.transform.position = Position;

        stone.GetComponent<ObjectInWorld>().Item = item;

        _diContainer.Inject(stone.GetComponent<ObjectInWorld>().Item);

        _itemsInWorld.PointsItems.Add(Position, stone.GetComponent<ObjectInWorld>());
    }
}
