using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadThisScenePlease(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
