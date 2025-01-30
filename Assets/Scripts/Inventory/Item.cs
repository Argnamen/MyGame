using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private ItemType _type;

    public Item(ItemType type)
    {
        _type = type;
    }
}

public enum ItemType
{
    Resources, Item
}
