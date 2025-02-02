using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bow : Weapon
{
    private readonly DiContainer _diContainer;
    private readonly IStaticDataService _staticDataService;
    private readonly ItemsInWorld _itemsInWorld;

    [Inject]
    public Bow(DiContainer diContainer, IStaticDataService staticDataService, ItemsInWorld itemsInWorld, TypeWeapon type, string id, int damage, float rangeDamage, float uptime, int corner)
        : base(type, id, damage, rangeDamage, uptime, corner)
    {
        _diContainer = diContainer;
        _staticDataService = staticDataService;
        _itemsInWorld = itemsInWorld;
    }

    public override bool CreateProjectile(Character spawner, Weapon weapon, Character target, Character[] targets)
    {
        GameObject newObject = _diContainer.InstantiatePrefab((GameObject)Resources.Load("Prefab/Arrow"), spawner.Position, Quaternion.identity, null);
        Projectile projectile = newObject.GetComponent<Arrow>().projectileObject = new Projectile(
            100,
            1,
            1f,
            new Weapon(TypeWeapon.Melee, "2", 10, 1, 0.1f, 10),
            spawner.Position,
            spawner.Environments,
            _staticDataService,
            _itemsInWorld);

        newObject.GetComponent<Arrow>().projectileObject.Target = target;
        newObject.GetComponent<Arrow>().EnemyList = new List<Character>(targets);

        _diContainer.Inject(newObject.GetComponent<Arrow>().projectileObject);

        return true;
    }
}

