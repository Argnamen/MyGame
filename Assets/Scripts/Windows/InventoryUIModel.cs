using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class InventoryUIModel : IInventoryUIModel
{
    private readonly InventoryUIView _inventoryUIViewView;

    public InventoryUIModel(InventoryUIView inventoryUIViewView)
    {
        _inventoryUIViewView = inventoryUIViewView;

        _inventoryUIViewView.CloseButton.onClick.AddListener(Close);
    }

    public void Open()
    {
        _inventoryUIViewView.Open();
    }

    public void Close()
    {
        _inventoryUIViewView.Close();
    }

    public void AddItem(Item item)
    {
        if(item.Type == ItemType.Resources)
        {
            if (item.ID == "Stone")
            {
                _inventoryUIViewView.StoneText.text = "Stone: " + item.Count;
            }
            else
            {
                _inventoryUIViewView.WoodText.text = "Wood: " + item.Count;
            }
        }
        else
        {
            _inventoryUIViewView.Items[0].ItemName.text = item.ID;
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        _inventoryUIViewView.Equip[0].ItemName.text = weapon.Id;
    }

    public void AddTool(Weapon weapon)
    {
        _inventoryUIViewView.Equip[1].ItemName.text = weapon.Id;
    }
}


public interface IInventoryUIModel
{
    public void Close();
}

