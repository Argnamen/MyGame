using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIView : MonoBehaviour
{
    public TMP_Text StoneText;
    public TMP_Text WoodText;

    public ItemUIView[] Items;
    public ItemUIView[] Equip;

    public Button CloseButton;

    public GameObject MainContainer;
    public GameObject WindowName;

    public void Close()
    {
        CloseButton.gameObject.SetActive(false);
        MainContainer.SetActive(false);
        WindowName.SetActive(false);
    }

    public void Open()
    {
        CloseButton.gameObject.SetActive(true);
        MainContainer.SetActive(true);
        WindowName.SetActive(true);
    }
}
