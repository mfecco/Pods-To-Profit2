using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;
    public void changeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void exitGame()
    {
       #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
       #else
        Application.Quit();
       #endif
    }
}
