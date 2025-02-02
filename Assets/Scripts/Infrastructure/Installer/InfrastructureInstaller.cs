using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InfrastructureInstaller : MonoInstaller
{
    [SerializeField] private GameObject curtainServicePrefab;

    public override void InstallBindings()
    { 
        BindServices();
        BindFactories();
    }

    private void BindServices()
    {
    Container.BindInterfacesAndSelfTo<CurtainService>()
      .FromComponentInNewPrefab(curtainServicePrefab)
      .WithGameObjectName("Curtain")
      .UnderTransformGroup("Infrastructure")
      .AsSingle().NonLazy();
    }

    private void BindFactories() 
    {
    }
} 

