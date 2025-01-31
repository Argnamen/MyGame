using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private string _id;
    private ItemType _type;

    public string ID => _id;
    public ItemType Type => _type;

    public Item(ItemType type, string id)
    {
        _type = type;
        _id = id;
    }
}

public enum ItemType
{
    Resources, Item
}
