using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeController : IGameMode
{
    private Action _stealsMod;
    private Action _fightMode;
    private Action _blockStealsMod;
    private Action _autoBattleMod;
    public Action StealsMod { get => _stealsMod; set { _stealsMod = value ;} }

    public Action FigthMod { get => _fightMode; set { _fightMode = value; } }

    public Action BlockStealsMod { get => _blockStealsMod; set { _blockStealsMod = value; } }

    public Action AutoBattleMod { get => _autoBattleMod; set { _autoBattleMod = value; } }

    private bool _isBlock = false;

    public void StealsModOn()
    {
        if(_isBlock)
            _fightMode?.Invoke();
        else
            _stealsMod?.Invoke();
    }

    public void FightModOn()
    {
        _fightMode?.Invoke();
    }

    public void BlockStealMode(bool isBlock)
    {
        _isBlock = isBlock;

        if (_isBlock)
        {
            _blockStealsMod?.Invoke();
            _fightMode?.Invoke();
        }
    }

    public void AutoBattleModOn()
    {
        _autoBattleMod?.Invoke();
    }
}

public interface IGameMode 
{
    public Action StealsMod { get; set; }
    public Action FigthMod { get; set; }
    public Action BlockStealsMod { get; set; }
    public Action AutoBattleMod { get; set; }

    public void StealsModOn();
    public void FightModOn();
    public void BlockStealMode(bool isBlock);
    public void AutoBattleModOn();
}
