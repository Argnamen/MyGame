public class BootstrapState : IState, IExitableState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IStaticDataService _staticDataService;

    public BootstrapState(GameStateMachine stateMachine, IStaticDataService staticDataService)
    {
        _stateMachine = stateMachine;
        _staticDataService = staticDataService;
    }

    public void Enter()
    {
        _staticDataService.Load();
        _stateMachine.Enter<LoadProgressState>();
    }

    public void Exit()
    {
        
    }
}


public interface IState
{
    void Enter();
    void Exit();
}

