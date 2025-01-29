using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WorldsMap
{
    private Dictionary<int, List<Vector3>> _worlds = new Dictionary<int, List<Vector3>>();

    public int CorrectWorld = 0;
    public int CurrentRoom = 0;

    [Inject]
    public void Constucrt()
    {
        
    }
    private void SetWorlds()
    {
        for(int i = 0; i < 10; i++)
        {
            _worlds.Add(i, new List<Vector3>());

            _worlds[i].Add(new Vector3(20, 20, 0));
            _worlds[i].Add(new Vector3(3, 3, 10));
            _worlds[i].Add(new Vector3(6, 6, 20));
            _worlds[i].Add(new Vector3(2, 2, 30));
        }
    }
    public List<Vector3> GetWorld(int number)
    {
        CorrectWorld = number;

        if(_worlds.Count == 0)
        {
            SetWorlds();
        }

        return _worlds[number];
    }
}
