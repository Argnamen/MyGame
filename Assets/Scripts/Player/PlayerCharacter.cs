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

    public Vector2 GetClosestEnemy()
    {
        Character centralEnemy = null;
        float minDistance = float.MaxValue;
        float lowestHP = float.MaxValue;

        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (this != null && EnemyList[i].HP > 0)
            {
                float distance = Vector2.Distance(Position, EnemyList[i].Position) - EnemyList[i].Size;

                if (centralEnemy == null || (distance < minDistance && minDistance > Weapon.Radius))
                {
                    minDistance = distance;
                    centralEnemy = EnemyList[i];
                }
                else if (centralEnemy == null || (minDistance <= Weapon.Radius && EnemyList[i].HP < lowestHP))
                {
                    centralEnemy = EnemyList[i];
                    lowestHP = EnemyList[i].HP;
                }
            }
        }

        return centralEnemy.Position;
    }
}



