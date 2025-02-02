using UnityEngine;

public class LoadProgressState : IState, IExitableState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IPersistentDataService _persistentDataService;
    private readonly IEconomyService _economyService;
    private readonly ILoggingService _loggingService;

    public LoadProgressState(
        GameStateMachine stateMachine,
        IPersistentDataService persistentDataService,
        IEconomyService economyService,
        ILoggingService loggingService)
    {
        _stateMachine = stateMachine;
        _persistentDataService = persistentDataService;
        _economyService = economyService;
        _loggingService = loggingService;
    }

    public void Enter()
    {
        _loggingService.LogInfo("Loading progress state entered.");
        LoadProgress();
        _stateMachine.Enter<LoadMetaState>();
    }

    public void Exit()
    {
        _loggingService.LogInfo("Exiting loading progress state.");
    }

    private void LoadProgress()
    {
        _loggingService.LogInfo("Loading player progress.");

        var progress = _persistentDataService.Load();
        _persistentDataService.SetProgress(progress);
        _economyService.Initialize(progress.EconomyData);
    }
}



