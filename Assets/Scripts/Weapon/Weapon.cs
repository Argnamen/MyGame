using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Weapon
{
    public int Damage;
    public float Radius;
    public float Uptime;
    public float Corner;

    public string Id;

    public TypeWeapon Type;

    public bool IsUptime = false;

    [Inject]
    protected DiContainer _diContainer;

    public Weapon (TypeWeapon type,string id, int damage, float rangeDamage, float Uptime, int corner)
    {
        this.Type = type;
        this.Id = id;
        this.Damage = damage;
        this.Radius = rangeDamage;
        this.Uptime = Uptime;

        if (corner < 0)
            Mathf.Abs(corner);

        if (corner > 360)
            corner = 360;

        this.Corner = corner;
    }

    public virtual bool CreateProjectile(Character spawner, Weapon weapon, Character target, Character[] targets)
    {
        return false;
    }

    public async void UpdateUptime()
    {
        IsUptime = true;

        await Task.Delay((int)(1000 * Uptime));

        IsUptime = false;
    }
}

public enum TypeWeapon
{
    Melee, Range, Magic
}
