using UnityEngine;

public class LoggingService : ILoggingService
{
    public void LogInfo(string message)
    {
        Debug.Log($"INFO: {message}");
    }

    public void LogWarning(string message)
    {
        Debug.LogWarning($"WARNING: {message}");
    }

    public void LogError(string message)
    {
        Debug.LogError($"ERROR: {message}");
    }
}

public interface ILoggingService
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message);
}

