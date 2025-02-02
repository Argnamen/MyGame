using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    private string _id;
    private ItemType _type;
    private Weapon _weapon;

    public string ID => _id;
    public ItemType Type => _type;
    public Weapon Weapon => _weapon;

    public Item(ItemType type, string id)
    {
        _type = type;
        _id = id;
    }

    public Item(Weapon weapon)
    {
        _type = ItemType.Item;
        _id = weapon.Id;
        _weapon = weapon;
    }
}

public enum ItemType
{
    Resources,
    Item
}

