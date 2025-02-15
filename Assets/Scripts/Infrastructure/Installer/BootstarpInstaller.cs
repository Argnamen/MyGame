using Dungeon;
using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;
using Cinemachine;

namespace Assets.Scripts.Infrastructure
{
    public class BootstarpInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private Player _playerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<DungeonCreator>().AsSingle();
            Container.Bind<DungeonList>().AsSingle();
            Container.Bind<MinionList>().AsSingle();
            Container.Bind<PlayerModels>().AsSingle();
            Container.Bind<Weapon>().AsSingle();
            Container.Bind<ItemsInWorld>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();

            Container.Bind<ILoggingService>().To<LoggingService>().AsSingle();
            Container.Bind<IPersistentDataService>().To<PersistentDataService>().AsSingle();
            Container.Bind<IEconomyService>().To<EconomyService>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            Container.Bind<IHeroFactory>().To<HeroFactory>().AsSingle();
            Container.Bind<IStageFactory>().To<StageFactory>().AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();

            Container.Bind<GameOverState>().AsSingle();

            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
            Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
            Container.Bind<CinemachineVirtualCamera>().FromInstance(_cinemachineVirtualCamera).AsSingle();
            Container.Bind<Player>().FromInstance(_playerPrefab).AsSingle();

        }
    }
}




