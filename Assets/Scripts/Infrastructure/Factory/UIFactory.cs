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
            Debug.Log("Кнопка видима");
        }
        else
        {
            Debug.Log("Кнопка не видима");
        }

        if (!okButton.resolvedStyle.visibility.Equals(Visibility.Hidden))
        {
            Debug.Log("Кнопка активна");
        }
        else
        {
            Debug.Log("Кнопка не активна");
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


