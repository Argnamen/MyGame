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
        //[SerializeField] private CinemachineInputProvider _cinemachineInputProvider;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        public override void InstallBindings()
        {
            Container.Bind<Test2>().AsSingle();
            Container.Bind<Test>().AsSingle();
            Container.Bind<DungeonCreator>().AsSingle();
            Container.Bind<DungeonList>().AsSingle();
            Container.Bind<Spawner>().AsSingle();
            Container.Bind<MinionList>().AsSingle();
            Container.Bind<PlayerModels>().AsSingle();
            Container.Bind<WorldsMap>().AsSingle();
            Container.Bind<Player>().AsSingle();
            Container.Bind<Weapon>().AsSingle();
            Container.Bind<ItemsInWorld>().AsSingle();

            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
            //Container.Bind<CinemachineInputProvider>().FromInstance(_cinemachineInputProvider).AsSingle();
            Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
            Container.Bind<CinemachineVirtualCamera>().FromInstance(_cinemachineVirtualCamera).AsSingle();
        }
    }
}



