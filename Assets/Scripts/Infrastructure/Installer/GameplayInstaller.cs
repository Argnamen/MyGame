using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _playerInputPrefab;
    [SerializeField] private CinemachineVirtualCamera _playerVirtualCamera;
    [SerializeField] private BattleUiView _battleUiView;

    public override void InstallBindings()
    {
        //BindInventory();
        BindTestCharacterFactory();


        Container.Bind(typeof(ISetupCamera), typeof(ICameraService)).To<CameraService>().AsSingle();
        Container.Bind<IGameMode>().To<GameModeController>().AsSingle();


        Container.Bind<BattleUiView>().FromComponentInNewPrefab(_battleUiView).AsTransient();
        Container.Bind<BattleUIModel>().AsTransient();
        Container.BindFactory<BattleUIModel, BattleUIModelFactory>().AsSingle();
    }

    private void BindInventory()
    {
        Container.BindInterfacesAndSelfTo<InventoryService>()
            .AsSingle()
            .NonLazy();
    }

    private void BindTestCharacterFactory()
    {
        Container.BindFactory<int, float, float, Weapon, Vector2, Environment[], TestCharacter, TestCharacter.Factory>()
            .FromFactory<CustomTestCharacterFactory>();
    }

    public class CustomTestCharacterFactory : IFactory<int, float, float, Weapon, Vector2, Environment[], TestCharacter>
    {
        readonly DiContainer _container;

        public CustomTestCharacterFactory(DiContainer container)
        {
            _container = container;
        }

        public TestCharacter Create(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Environment[] environments)
        {
            var character = new TestCharacter(healt, spead, size, weapon, startPos, environments);
            _container.Inject(character);
            return character;
        }
    }

}




