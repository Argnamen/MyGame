using System.Collections.Generic;
using UnityEngine;

public class StaticDataService : IStaticDataService
{
    private List<List<Vector3>> _worlds = new List<List<Vector3>>();
    public int CurrentRoom { get; set; }

    public StaticDataService()
    {
        LoadWorlds();
    }

    private void LoadWorlds()
    {
        _worlds.Add(new List<Vector3> { new Vector3(10, 10, 0), new Vector3(20, 20, 0) });
        _worlds.Add(new List<Vector3> { new Vector3(15, 15, 0), new Vector3(25, 25, 0) });
        CurrentRoom = 0;
    }

    public void Load()
    {
        LoadWorlds();
    }

    public List<Vector3> GetWorld(int worldId)
    {
        if (worldId >= 0 && worldId < _worlds.Count)
        {
            return _worlds[worldId];
        }
        return null;
    }
}

public interface IStaticDataService
{
    List<Vector3> GetWorld(int worldId);
    void Load();
    int CurrentRoom { get; set; }
}



