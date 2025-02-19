using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUiView : MonoBehaviour
{
    public Slider slider;
    public TMP_Text text;

    public Button InventoryOpen;

    public void UpdateHealth(int health)
    {
        text.text = $"Health: {health}";
        slider.value = 1 - health / 100;
    }
}

