using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Bow : Weapon
{
    public Bow(TypeWeapon type, string id, int damage, float rangeDamage, float Uptime, int corner) : base(type, id, damage, rangeDamage, Uptime, corner)
    {
    }

    public override bool CreateProjectile(Character spawner, Weapon weapon, Character target, Character[] targets)
    {
        GameObject newObject = _diContainer.InstantiatePrefab((GameObject)Resources.Load("Prefab/Arrow"), spawner.Position, Quaternion.identity, null);

        Projectile projectile = newObject.GetComponent<Arrow>().projectileObject = new Projectile(100, 1, 1f, new Weapon(TypeWeapon.Melee, "2", 10, 1, 0.1f, 10), spawner.Position, spawner.Environments);

        newObject.GetComponent<Arrow>().projectileObject.Target = target;
        newObject.GetComponent<Arrow>().EnemyList = new List<Character>(targets);

        _diContainer.Inject(newObject.GetComponent<Arrow>().projectileObject);

        return true;
    }
}
