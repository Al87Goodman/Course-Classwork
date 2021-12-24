using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Capture state
    private bool isPaused;

    // Display Panels (Pause, Crystal Bag, and Save) 
    public GameObject pausePanel;
    public GameObject crystalPanel;
    public GameObject savePanel;
    public GameObject infoPanel;
    public bool usingPausePanel;

    public string mainMenu;

    // Hide the Pause and Crystal Menus on Start
    void Start()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        crystalPanel.SetActive(false);
        savePanel.SetActive(false);
        infoPanel.SetActive(false);
        usingPausePanel = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Set in Project Settings/InputManager
        if (Input.GetButtonDown("pause"))
        {
            FlipPause();
        }
    }

    // Buttons on the Pause Panel
   public void FlipPause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f; // change in future to effect time differently (mutliple by 0)
            usingPausePanel = true;
        }
        else
        {
            crystalPanel.SetActive(false);
            savePanel.SetActive(false);
            pausePanel.SetActive(false);
            infoPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
   public void QuitToMain()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f; // Animations run off of this
        SceneDataManager.MyInstance.DeleteSceneData(); // removes the temp scene data before loading main menu
    }

    // Switch between Pause and Crystal Panels
    public void SwitchPanels()
    {
        usingPausePanel = !usingPausePanel;
        if (usingPausePanel)
        {
            pausePanel.SetActive(true);
            crystalPanel.SetActive(false);
            savePanel.SetActive(false);
            infoPanel.SetActive(false);
        }
        else
        {
            crystalPanel.SetActive(true);
            pausePanel.SetActive(false);
            savePanel.SetActive(false);
            infoPanel.SetActive(false);
        }
    }
    public void SwitchToSave()
    {
        usingPausePanel = false;
        savePanel.SetActive(true);
        crystalPanel.SetActive(false);
        pausePanel.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void SwitchToPause()
    {
        usingPausePanel = true;
        savePanel.SetActive(false);
        crystalPanel.SetActive(false);
        pausePanel.SetActive(true);
        infoPanel.SetActive(false);
    }

    // Display Game Information Such as Controls & Hints
    public void SwitchToInfo()
    {
        usingPausePanel = false;
        infoPanel.SetActive(true);
        savePanel.SetActive(false);
        crystalPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

}
