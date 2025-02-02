using System.Collections.Generic;

[System.Serializable]
public class PlayerProgress
{
    public int Level { get; set; }
    public int ExperiencePoints { get; set; }
    public Inventory Inventory { get; set; }
    public List<string> Achievements { get; set; }
    public EconomyData EconomyData { get; set; }

    public PlayerProgress()
    {
        Level = 1;
        ExperiencePoints = 0;
        Inventory = new Inventory();
        Achievements = new List<string>();
        EconomyData = new EconomyData();
    }
}

