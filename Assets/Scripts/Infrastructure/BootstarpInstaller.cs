using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Dungeon;

namespace Assets.Scripts.Infrastructure
{
    public class BootstarpInstaller: MonoInstaller
    {
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

            Container.Bind<Character>().To<TestCharacter>().AsSingle();
        }
    }
}
