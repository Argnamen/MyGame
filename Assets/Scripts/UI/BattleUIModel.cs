using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIModel : IBattleUIModel
{
    private readonly BattleUiView _battleUiView;

    public BattleUIModel(BattleUiView battleUiView)
    {
        _battleUiView = battleUiView;
    }

    public void UpdateHealth(int health)
    {
        _battleUiView.UpdateHealth(health);
    }
}


public interface IBattleUIModel
{
    void UpdateHealth(int health);
}

