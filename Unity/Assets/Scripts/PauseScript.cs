using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
    }
    public void ResumeFromButton()
    {
        Resume();
        EventSystem.current.SetSelectedGameObject(null);
    }
}

