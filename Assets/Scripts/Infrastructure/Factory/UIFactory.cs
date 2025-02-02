using UnityEngine;

public class UIFactory : IUIFactory
{
    public void CreateUI()
    {
        Debug.Log("UI created successfully.");
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


