using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerCharacter : Character
{
    public List<Character> EnemyList = new List<Character>();

    public PlayerCharacter(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Environment[] environments, IStaticDataService staticDataService, ItemsInWorld itemsInWorld)
        : base(healt, spead, size, weapon, startPos, environments, staticDataService, itemsInWorld)
    {
    }

    public override Vector2 Move(Direction direction)
    {
        return base.Move(direction);
    }
}



