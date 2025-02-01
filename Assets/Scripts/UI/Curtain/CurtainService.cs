using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainService : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowCurtain()
    {
        // ��� ��� ��� ������ �������� (��������, �������� ��� ��������� �����-������)
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void HideCurtain()
    {
        // ��� ��� ��� ������� �������� (��������, �������� ��� ��������� �����-������)
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
