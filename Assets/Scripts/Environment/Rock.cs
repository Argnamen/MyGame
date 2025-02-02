using UnityEngine;

public class Rock : EnemyCharacter
{
    public Rock(
        int healt,
        float spead,
        float size,
        Weapon weapon,
        Vector2 startPos,
        Character playerCharacter,
        Environment[] environments,
        IStaticDataService staticDataService,
        ItemsInWorld itemsInWorld)
        : base(healt, spead, size, weapon, startPos, playerCharacter, null, 0, environments, staticDataService, itemsInWorld)
    {
    }
}



