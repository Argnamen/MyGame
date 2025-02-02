using UnityEngine;

public class GameLoopState : IState, IExitableState
{
    private readonly GameStateMachine _stateMachine;
    private readonly Player _player;
    private readonly IEnemyFactory _enemyFactory;

    public GameLoopState(GameStateMachine stateMachine, Player player, IEnemyFactory enemyFactory)
    {
        _stateMachine = stateMachine;
        _player = player;
        _enemyFactory = enemyFactory;
    }

    public void Enter()
    {
        StartGameLoop();
    }

    public void Exit()
    {
        
    }

    private void StartGameLoop()
    {
        _player.OnPlayerDied += OnPlayerDied;

        _enemyFactory.CreateEnemies(10);

        Debug.Log("Game loop started.");
    }

    private void OnPlayerDied()
    {
        _player.OnPlayerDied -= OnPlayerDied;
        _stateMachine.Enter<GameOverState>();
    }
}



