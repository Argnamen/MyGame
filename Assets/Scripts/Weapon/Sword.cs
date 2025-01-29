using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public Sword(TypeWeapon type, string id, int damage, float radius, float Uptime, int corner) : base(type, id, damage, radius, Uptime, corner)
    {
    }
}
