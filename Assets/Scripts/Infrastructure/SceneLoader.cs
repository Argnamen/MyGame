using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public void LoadScene(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);
        Debug.Log($"Scene {sceneName} loaded successfully.");
    }
}

