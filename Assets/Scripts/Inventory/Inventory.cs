using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> _items = new List<Item>();
    public void AddItem(Item item)
    {
        _items.Add(item);

        Debug.Log($"Items In Inventory: {_items.Count} Item 0 count:{_items.FindAll(x => x == _items[0]).Count}");
    }

    public Item[] GetAllItems()
    {
        if (_items.Count == 0)
            return null;

        return _items.ToArray();
    }

    public Item DropItem(Item item)
    {
        return _items.Find(x => x == item);
    }
}
