using UnityEngine;

public class UIFactory : IUIFactory
{
    private BattleUIModel _battleUIModelFactory;
    private IHeroFactory _heroFactory;
    public UIFactory(BattleUIModel battleUIModelFactory, IHeroFactory heroFactory)
    {
        _battleUIModelFactory = battleUIModelFactory;
        _heroFactory = heroFactory;
    }
    public void CreateUI()
    {
        _battleUIModelFactory.UpdateHealth(_heroFactory.GetHero().Character.HP);
        _heroFactory.GetHero().Character.DamageEvent.AddListener(() => _battleUIModelFactory.UpdateHealth(_heroFactory.GetHero().Character.HP));
    }

    public void CreateGameOverUI()
    {
        Debug.Log("Game over UI created successfully.");
    }
}


public interface IUIFactory
{
    void CreateUI();
    void CreateGameOverUI();
}


