/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**********************************************
 * Class to handle player party on overworld
 * and switch between active/inactive.
 **********************************************/
public class StartBattle : MonoBehaviour
{
    //[SerializeField]
    //private string battleName;

    void Start()
    {
        //don't destroy this object when new scene is loaded
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        //by default, don't have this object active
        //switches to active under certain conditions
        this.gameObject.SetActive(false);        
    }

    //Method to check which scene it is
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if in Title screen, destroy this object
        if(scene.name == "StartMenu")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(this.gameObject);
        }
        //if in a town or wilds, make inactive
        else if(scene.name == "BasicTown" || scene.name == "Wild")
        {
            this.gameObject.SetActive(false);
        }
        //if in a battle, make active
        else if(scene.name == "Sample_Battle" || scene.name == "City_Battle" || scene.name == "Coast_Battle" || scene.name == "Desert_Battle" || scene.name == "Forest_Battle" || scene.name == "Plains_Battle")
        {
            this.gameObject.SetActive(true);
        }
    }
}
