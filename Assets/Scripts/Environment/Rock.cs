using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Enemy
{
    public Rock(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Player player, Minion[] minion, int numberMinion, Environment[] environments) : base(healt, spead, size, weapon, startPos, player, minion, numberMinion, environments)
    {
    }
}
