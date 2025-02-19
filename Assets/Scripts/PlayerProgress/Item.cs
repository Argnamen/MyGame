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

    public int Count;

    public Item(ItemType type, string id, int count)
    {
        _type = type;
        _id = id;

        Count = count;
    }

    public Item(Weapon weapon)
    {
        _type = ItemType.Item;
        _id = weapon.Id;
        _weapon = weapon;

        Count = 1;
    }
}

public enum ItemType
{
    Resources,
    Item
}

