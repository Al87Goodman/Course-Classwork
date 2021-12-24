using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject savePanel;
    public GameInfo gameInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Main Menu Buttons
    public void NewGame()
    {
        // Set the Scriptable Object to True. This is referenced in CharacterPosition script and Panel
        gameInfo.NewGame = true;
        SceneManager.LoadScene("BasicTown");
    }

    public void LoadContinue()
    {
        savePanel.SetActive(true);
    }

    public void QuitExit()
    {
        Application.Quit();
    }

}
