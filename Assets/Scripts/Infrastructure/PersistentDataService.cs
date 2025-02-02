using System.IO;
using UnityEngine;

public class PersistentDataService : IPersistentDataService
{
    private const string SaveFilePath = "player_progress.json";
    private PlayerProgress _currentProgress;

    public PlayerProgress Load()
    {
        _currentProgress = LoadFromFile();
        return _currentProgress;
    }

    public void Save(PlayerProgress progress)
    {
        _currentProgress = progress;
        SaveToFile(progress);
    }

    public void SetProgress(PlayerProgress progress)
    {
        _currentProgress = progress;
    }

    public PlayerProgress GetProgress()
    {
        return _currentProgress;
    }

    private PlayerProgress LoadFromFile()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning("Save file not found, creating new progress.");
            return new PlayerProgress();
        }

        try
        {
            string json = File.ReadAllText(SaveFilePath);
            return JsonUtility.FromJson<PlayerProgress>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load progress: {e.Message}");
            return new PlayerProgress();
        }
    }

    private void SaveToFile(PlayerProgress progress)
    {
        try
        {
            string json = JsonUtility.ToJson(progress, true);
            File.WriteAllText(SaveFilePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save progress: {e.Message}");
        }
    }
}



public interface IPersistentDataService
{
    PlayerProgress Load();
    void Save(PlayerProgress progress);
    void SetProgress(PlayerProgress progress);
    PlayerProgress GetProgress();
}

