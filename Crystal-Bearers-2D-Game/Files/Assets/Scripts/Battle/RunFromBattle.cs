/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*******************************************
 * Class for handling running from battle
 ******************************************/
public class RunFromBattle : MonoBehaviour
{
    //percent chance to run
    [SerializeField]
    private double runChance;

    //scene to run to
    [SerializeField]
    private string runScene;
    public GameInfo gameInfo;

    public void tryRunning()
    {
        System.Random rand = new System.Random();
        double randRun = rand.NextDouble();

        //if the random run number is less than the run chance, party flees
        if(randRun < this.runChance)
        {
            // Save the updated Spirit Data and Dismiss Spirit Objects
            PlayerPartyManager.MyInstance.DismissSpirits();

            Debug.Log("Spirits Dismissed R");
            runScene = gameInfo.FleeLocation;
            Debug.Log("FLEE LOCATION - " + runScene);
            //Load run scene
            SceneDataManager.MyInstance.SceneTransition(runScene);
        }
        //if not, the turn is wasted and it goes to the next turn
        else
        {
            GameObject.Find("TurnSystem").GetComponent<TurnSystem>().nextTurn();
        }

    }
}
