using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> _items = new List<Item>();
    public void AddItem(Item item)
    {
        _items.Add(item);

        Debug.Log($"Items In Inventory: {_items.Count}");
    }

    public Item[] GetAllItems()
    {
        return _items.ToArray();
    }
}
