using UnityEngine;

public class EnemyCharacter : Character
{
    public Character Target { get; private set; }
    private Minion[] _minions;
    private int _numberMinion;

    public EnemyCharacter(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Character target, Minion[] minions, int numberMinion, Environment[] environments, IStaticDataService staticDataService, ItemsInWorld itemsInWorld)
        : base(healt, spead, size, weapon, startPos, environments, staticDataService, itemsInWorld)
    {
        Target = target;
        _minions = minions;
        _numberMinion = numberMinion;
    }

    public Vector2 MoveToTarget(float distance)
    {
        Vector2 direction = (Target.Position - Position).normalized;
        return Position + direction * distance;
    }
}



