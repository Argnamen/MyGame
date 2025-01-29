using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    public TestEnemy(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Player player, Minion[] minions, int numberMinion, Environment[] environments) 
        : base(healt, spead, size, weapon, startPos, player, minions, numberMinion, environments)
    {
        
    }
}
