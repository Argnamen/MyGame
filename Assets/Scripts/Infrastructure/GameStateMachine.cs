using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Zenject;

public class GameStateMachine : IInitializable
{
    private readonly ILoggingService _loggingService;
    private readonly DiContainer _diContainer;
    private IState _currentState;

    public GameStateMachine(ILoggingService loggingService, DiContainer diContainer)
    {
        _loggingService = loggingService;
        _diContainer = diContainer;
    }

    public void Initialize()
    {
        _loggingService.LogInfo("GameStateMachine initialized.");
        Enter<LoadProgressState>();
    }

    public void Enter<TState>() where TState : IState
    {
        _currentState?.Exit();

        var state = _diContainer.Resolve<TState>();
        _loggingService.LogInfo($"Entering state: {typeof(TState).Name}");
        _currentState = state;
        _currentState.Enter();
    }
}



public interface IExitableState
{
    void Exit();
}

