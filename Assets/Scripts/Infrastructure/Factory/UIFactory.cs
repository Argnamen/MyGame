using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIFactory : IUIFactory
{
    private BattleUIHierechy _battleUIHierechy;
    public UIFactory(BattleUIHierechy battleUIHierechy) 
    {
        _battleUIHierechy = battleUIHierechy;
    }
    public void CreateUI()
    {
        GameObject UIObject = MonoBehaviour.Instantiate(_battleUIHierechy.gameObject);

        Button okButton = UIObject.GetComponent<BattleUIHierechy>().UIdocument.rootVisualElement.Q<Button>("okButton");

        okButton.clicked += () => { Debug.Log("ok AAAAA"); };

        okButton.RegisterCallback<ClickEvent>(ClickMessage);

        if (!okButton.resolvedStyle.display.Equals(DisplayStyle.None))
        {
            Debug.Log("������ ������");
        }
        else
        {
            Debug.Log("������ �� ������");
        }

        if (!okButton.resolvedStyle.visibility.Equals(Visibility.Hidden))
        {
            Debug.Log("������ �������");
        }
        else
        {
            Debug.Log("������ �� �������");
        }
    }
    void ClickMessage(ClickEvent e)
    {
        Debug.Log("ok BBBBB");
    }

    public void CreateGameOverUI()
    {
        Debug.Log("Game over UI created successfully.");
    }
}


public interface IUIFactory
{
    void CreateUI();
    void CreateGameOverUI();
}


