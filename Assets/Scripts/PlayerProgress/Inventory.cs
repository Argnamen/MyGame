using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Inventory
{
    private List<Item> _items = new List<Item>();

    public void AddItem(Item item)
    {
        Item itemInv = _items.Find(x => x.ID == item.ID);

        if (itemInv != null)
        {
            itemInv.Count += item.Count;
        }
        else
        {
            itemInv = item;
        }

        _items.Add(item);
        Debug.Log($"Items In Inventory: {itemInv.Count} Item 0 count:{itemInv.Count}");
    }

    public Item GetItem(Item item)
    {
        Item itemInv = _items.Find(x => x.ID == item.ID);

        if (itemInv == null)
        {
            itemInv = item;
        }

        return itemInv;
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

