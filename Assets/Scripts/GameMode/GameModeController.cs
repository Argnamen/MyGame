using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeController : IGameMode
{
    private GameMod mod = GameMod.Peace;

    private Action _stealsMod;
    private Action _returnMod;
    private Action _peaceMod;
    private Action _blockPeaceMods;
    private Action _autoBattleMod;
    public Action StealthMod { get => _stealsMod; set { _stealsMod = value ;} }

    public Action ReturnMod { get => _returnMod; set { _returnMod = value; } }

    public Action PeaceMod { get => _peaceMod; set { _peaceMod = value; } }

    public Action AutoBattleMod { get => _autoBattleMod; set { _autoBattleMod = value; } }

    private bool _isBlock = false;

    public void StealsModOn()
    {
        if (_isBlock)
        {
            mod = GameMod.Fight;
            _returnMod?.Invoke();
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
            _returnMod?.Invoke();
        }
        else
        {
            mod = GameMod.Peace;
            _peaceMod?.Invoke();
        }
    }

    public void ReturnLastMod()
    {
        _returnMod?.Invoke();
    }

    public void BlockPeaceMods(bool isBlock)
    {
        _isBlock = isBlock;

        if (_isBlock)
        {
            mod = GameMod.Fight;
            _returnMod?.Invoke();
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
    public Action ReturnMod { get; set; }
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
