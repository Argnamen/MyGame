using UnityEngine;

public class GameOverState : IState, IExitableState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IUIFactory _uiFactory;

    public GameOverState(GameStateMachine stateMachine, IUIFactory uiFactory)
    {
        _stateMachine = stateMachine;
        _uiFactory = uiFactory;
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
