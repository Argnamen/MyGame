using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestCharacter : Character
{
    private ItemsInWorld _itemsInWorld;
    [Inject]
    public void Construct(ItemsInWorld itemsInWorld)
    {
        _itemsInWorld = itemsInWorld;
    }

    public TestCharacter(int healt, float spead,float size, Weapon weapon, Vector2 startPos, Environment[] environments) : base(healt, spead, size, weapon, startPos, environments)
    {

    }

    public override Vector2 Move(Direction direction)
    {
        foreach (var item in _itemsInWorld.PointsItems)
        {
            if(Vector2.Distance(item.Key, Position) <= Size)
            {
                Inventory.AddItem(item.Value.Item);

                item.Value.DeathAction();

                _itemsInWorld.PointsItems.Remove(item.Key);

                break;
            }
        }

        return base.Move(direction);
    }
}
