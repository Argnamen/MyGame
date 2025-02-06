using UnityEngine;
using UnityEngine.UIElements;
public class LoadLevelState : IState, IExitableState
{
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly IUIFactory _uiFactory;
    private readonly IHeroFactory _heroFactory;
    private readonly IStageFactory _stageFactory;

    public LoadLevelState(
        GameStateMachine stateMachine,
        SceneLoader sceneLoader,
        IUIFactory uiFactory,
        IHeroFactory heroFactory,
        IStageFactory stageFactory)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _uiFactory = uiFactory;
        _heroFactory = heroFactory;
        _stageFactory = stageFactory;
    }

    public void Enter()
    {
        LoadLevel();
    }

    public void Exit()
    {
        
    }

    private void LoadLevel()
    {
        _sceneLoader.LoadScene("GameScene");
        _uiFactory.CreateUI();
        _stageFactory.CreateStage();

        _stateMachine.Enter<GameLoopState>();

        Debug.Log("Level loaded successfully.");
    }
}


