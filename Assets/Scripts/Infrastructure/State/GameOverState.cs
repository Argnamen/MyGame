using UnityEngine;
using UnityEngine.UIElements;

public class GameOverState : IState, IExitableState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IUIFactory _uiFactory;
    private UIDocument _uiDocument;

    public GameOverState(GameStateMachine stateMachine, IUIFactory uiFactory, UIDocument uIDocument)
    {
        _stateMachine = stateMachine;
        _uiFactory = uiFactory;
        _uiDocument = uIDocument;
    }

    public void Enter()
    {
        ShowGameOverScreen();
    }

    public void Exit()
    {
        
    }

    private void ShowGameOverScreen()
    {
        _uiFactory.CreateGameOverUI();

        Debug.Log("Game over. Displaying options to restart or return to the main menu.");
    }
}
