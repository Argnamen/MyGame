using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
        return _items.Count == 0 ? null : _items.ToArray();
    }

    public Item DropItem(Item item)
    {
        var itemToDrop = _items.Find(x => x == item);
        if (itemToDrop != null)
        {
            _items.Remove(itemToDrop);
            return itemToDrop;
        }
        return null;
    }
}

