using Zenject;

public class BattleUIModelFactory : PlaceholderFactory<BattleUIModel>
{
    private readonly DiContainer _container;

    public BattleUIModelFactory(DiContainer container)
    {
        _container = container;
    }

    public override BattleUIModel Create()
    {
        var battleUiViewPrefab = _container.Resolve<BattleUiView>();
        var battleUiViewInstance = _container.InstantiatePrefabForComponent<BattleUiView>(battleUiViewPrefab.gameObject);
        return new BattleUIModel(battleUiViewInstance);
    }
}





