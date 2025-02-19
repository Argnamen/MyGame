using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryUIModelFactory : PlaceholderFactory<InventoryUIModel>
{

    private readonly DiContainer _container;

    public InventoryUIModelFactory(DiContainer container)
    {
        _container = container;
    }

    public override InventoryUIModel Create()
    {
        var ViewPrefab = _container.Resolve<InventoryUIView>();
        var ViewInstance = _container.InstantiatePrefabForComponent<InventoryUIView>(ViewPrefab.gameObject);
        return new InventoryUIModel(ViewInstance);
    }

}
