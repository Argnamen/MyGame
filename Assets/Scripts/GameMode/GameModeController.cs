using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeController : IGameMode
{
    private GameMod mod = GameMod.Peace;

    private Action _stealsMod;
    private Action _fightMode;
    private Action _peaceMod;
    private Action _blockPeaceMods;
    private Action _autoBattleMod;
    public Action StealthMod { get => _stealsMod; set { _stealsMod = value ;} }

    public Action FightMod { get => _fightMode; set { _fightMode = value; } }

    public Action PeaceMod { get => _peaceMod; set { _peaceMod = value; } }

    public Action AutoBattleMod { get => _autoBattleMod; set { _autoBattleMod = value; } }

    private bool _isBlock = false;

    public void StealsModOn()
    {
        if (_isBlock)
        {
            mod = GameMod.Fight;
            _fightMode?.Invoke();
        }
        else
        {
            mod = GameMod.Peace;
            _stealsMod?.Invoke();
        }
    }

    public void PeaceModOn()
    {
        if (_isBlock)
        {
            mod = GameMod.Fight;
            _fightMode?.Invoke();
        }
        else
        {
            mod = GameMod.Peace;
            _peaceMod?.Invoke();
        }
    }

    public void ReturnLastMod()
    {
        switch (mod)
        {
            case GameMod.Peace:
                _peaceMod?.Invoke();
                break;
            case GameMod.Fight:
                _fightMode?.Invoke();
                break;
        }
    }

    public void BlockPeaceMods(bool isBlock)
    {
        _isBlock = isBlock;

        if (_isBlock)
        {
            mod = GameMod.Fight;
            _fightMode?.Invoke();
        }
        else
        {
            mod = GameMod.Peace;
            _peaceMod?.Invoke();
        }
    }

    public void AutoBattleModOn()
    {
        _autoBattleMod?.Invoke();
    }
}

public interface IGameMode 
{
    public Action StealthMod { get; set; }
    public Action FightMod { get; set; }
    public Action PeaceMod { get; set; }
    public Action AutoBattleMod { get; set; }

    public void StealsModOn();
    public void ReturnLastMod();
    public void PeaceModOn();
    public void BlockPeaceMods(bool isBlock);
    public void AutoBattleModOn();
}

public enum GameMod
{
    Peace, Fight
}
