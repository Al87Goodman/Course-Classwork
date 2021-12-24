/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectUnit : MonoBehaviour
{
    //store object of current fighter
    private GameObject currentUnit;

    private GameObject actionsMenu;

    private GameObject enemyUnitsMenu;


    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Method to load in proper pieces needed for battle
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //only run if in Battle scene
        if(scene.name == "Sample_Battle" || scene.name == "City_Battle" || scene.name == "Coast_Battle" || scene.name == "Desert_Battle" || scene.name == "Forest_Battle" || scene.name == "Plains_Battle" || scene.name == "Aborius_Boss" || scene.name == "Hagitha_Boss" || scene.name == "Marina_Boss" || scene.name == "Ruby_Boss" || scene.name == "Toxitra_Boss")
        {
            this.actionsMenu = GameObject.Find("ActionMenu");
            this.enemyUnitsMenu = GameObject.Find("EnemyUnitsMenu");
        }
    }

    //Method to activate current unit
    public void selectCurrentUnit(GameObject unit)
    {
        this.currentUnit = unit;
        this.actionsMenu.SetActive(true);
        this.currentUnit.GetComponent<PlayerUnitAction>().updateHUD();
    }

    //Method to set physical or magical attack
    public void selectAttack(bool isPhysical)
    {
        this.currentUnit.GetComponent<PlayerUnitAction>().selectAttack(isPhysical);

        this.actionsMenu.SetActive(false);
        this.enemyUnitsMenu.SetActive(true);
    }

    //Method to deliver attack
    public void attackEnemyTarget(GameObject target)
    {
        this.actionsMenu.SetActive(false);
        this.enemyUnitsMenu.SetActive(false);

        this.currentUnit.GetComponent<PlayerUnitAction>().act(target);
    }
}
