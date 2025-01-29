using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Dungeon
{
    public class DungeonList
    {
        [Inject]
        public void Construct()
        {

        }

        public void Survivors()
        {
            Vector2[] spawnPoints = new Vector2[4];

            float round = 10;

            spawnPoints[0] = Vector2.zero;
            spawnPoints[1] = Vector2.right * round;
            spawnPoints[2] = Vector2.left * round;
            spawnPoints[3] = Vector2.one * round;
        }
    }
}
