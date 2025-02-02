using UnityEngine;
public class LoadMetaState : IState, IExitableState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IUIFactory _uiFactory;
    private readonly SceneLoader _sceneLoader;
    private readonly IHeroFactory _heroFactory;
    private readonly IStageFactory _stageFactory;

    public LoadMetaState(
        GameStateMachine stateMachine,
        IUIFactory uiFactory,
        SceneLoader sceneLoader,
        IHeroFactory heroFactory,
        IStageFactory stageFactory)
    {
        _stateMachine = stateMachine;
        _uiFactory = uiFactory;
        _sceneLoader = sceneLoader;
        _heroFactory = heroFactory;
        _stageFactory = stageFactory;
    }

    public void Enter()
    {
        LoadMeta();
        _stateMachine.Enter<LoadLevelState>();
    }

    public void Exit()
    {
        
    }

    private void LoadMeta()
    {
        _uiFactory.CreateUI();
        _sceneLoader.LoadScene("MainMenuScene");

        Debug.Log("Meta data loaded successfully.");
    }
}


