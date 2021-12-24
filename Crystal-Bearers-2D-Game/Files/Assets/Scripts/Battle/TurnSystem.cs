/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*****************************************
 * Class to handle the turns in battle
 *****************************************/
public class TurnSystem : MonoBehaviour
{
    private List<UnitStats> unitStats;

    [SerializeField]
    private GameObject damageTextPrefab;
    [SerializeField]
    private Vector2 damageTextPosition;

    //the player party
    private GameObject playerParty;

    private GameObject partyMembers;

    // The Game State Information
    public GameInfo gameInfo;

    // The Enemy Party
    private GameObject[] enemyMembers;

    //the enemy party
    public GameObject enemyEncounter;

    //menu of party actions
    [SerializeField]
    private GameObject actionsMenu;

    //enemy targeting menu
    [SerializeField]
    private GameObject enemyUnitsMenu;

    //scene to load on win
    [SerializeField]
    private string winScene;

    //scene to load on loss
    [SerializeField]
    private string lossScene;

    //battle end bool
    bool battleDone;

    //Space pressed
    bool spacePress;

    void Start()
    {
        StartCoroutine("StartTurn");
    }

    IEnumerator StartTurn()
    {
        yield return new WaitForSeconds(1);

        //get player party object
        this.playerParty = GameObject.Find("Player Party");

        unitStats = new List<UnitStats>();

        //Load in party member objects
        PlayerPartyManager.MyInstance.SummonSpirits();

        //Get the Player Party
        GameObject[] partyMembers = GameObject.FindGameObjectsWithTag("PartyUnit");

        Debug.Log("Turn System (Party Member Count): " + partyMembers.Length);

        //set fields for each party member
        foreach (GameObject partyMember in partyMembers)
        {
            UnitStats currentUnitStats = partyMember.GetComponent<UnitStats>();
            currentUnitStats.calculateNextActTurn(0);
            unitStats.Add(currentUnitStats);
        }

        //create enemy party
        enemyMembers = GameObject.FindGameObjectsWithTag("EnemyUnit");

        //set fields for enemy units
        foreach (GameObject enemyUnit in enemyMembers)
        {
            UnitStats currentUnitStats = enemyUnit.GetComponent<UnitStats>();
            currentUnitStats.calculateNextActTurn(0);
            unitStats.Add(currentUnitStats);
        }

        //sort unitStats list to organize turn order
        unitStats.Sort();

        //set flags to false
        //this.actionsMenu.SetActive(false);
        this.enemyUnitsMenu.SetActive(false);

        battleDone = false;
        spacePress = false;

        //run the next turn
        this.nextTurn();
    }

    

    //Method to handle next turn
    public void nextTurn()
    {
        Debug.Log("New Turn");

        //check if there are remaining enemies
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("EnemyUnit");

        //end battle and distribute rewards if all enemies dead
        if(remainingEnemies.Length == 0)
        {
            if(battleDone == false)
            {
                battleDone = true;

                this.enemyEncounter.GetComponent<CollectReward>().collectReward();
                this.enemyEncounter.GetComponent<CollectReward>().CollectCrystal();

                Debug.Log("Battle Win");

                //display battle win message
                GameObject winMessage = GameObject.Find("Battle Message");
                winMessage.GetComponent<Text>().text = "Battle Won!";

                StartCoroutine("winning");       
            }
            else
            {
                return;
            }

        }

        //check if all party members are dead
        GameObject[] remainingParty = GameObject.FindGameObjectsWithTag("PartyUnit");
        //if all party members dead, go to game over screen
        if (remainingParty.Length == 0)
        {
            if(battleDone == false)
            {
                battleDone = true;

                // Remove Half of the Players Coins
                this.enemyEncounter.GetComponent<CollectReward>().HalveCoins();
                Debug.Log("Battle Loss");

                //display battle loss message
                GameObject lossMessage = GameObject.Find("Battle Message");
                lossMessage.GetComponent<Text>().text = "Party Wiped Out...";

                StartCoroutine("losing");    
            }
            else
            {
                return;
            }

        }

        //load in stats of current fighter
        UnitStats currentUnitStats = unitStats[0];
        unitStats.Remove(currentUnitStats);

        //make sure fighter is not dead
        if (!currentUnitStats.isDead())
        {
            GameObject currentUnit = currentUnitStats.gameObject;

            currentUnitStats.calculateNextActTurn(currentUnitStats.nextActTurn);
            unitStats.Add(currentUnitStats);
            unitStats.Sort();

            //check if current turn is party member
            if (currentUnit.tag == "PartyUnit")
            {
                Debug.Log("Party Attack");
                this.playerParty.GetComponent<SelectUnit>().selectCurrentUnit(currentUnit.gameObject);
            }
            //if enemy instead, use enemy attack
            else
            {
                Debug.Log("Enemy Attack");
                currentUnit.GetComponent<EnemyUnitAction>().act();
            }
        }
        //if dead, skip turn
        else
        {
            this.nextTurn();
        }
    }

    IEnumerator winning()
    {
        yield return new WaitForSeconds(2);

        // Save the updated Spirit Data and Dismiss Spirit Objects
        PlayerPartyManager.MyInstance.DismissSpirits();

        Debug.Log("Spirits Dismissed W");

        // Load WinScene
        // Will Need to Call the SceneDataManager to transition to save data.
        SceneDataManager.MyInstance.SceneTransition(winScene);
        Debug.Log("Scene Transition W");
    }

    IEnumerator losing()
    {
        yield return new WaitForSeconds(2);

        // Save the updated Spirit Data and Dismiss Spirit Objects. Takes a bool to determine if full heal or not.
        PlayerPartyManager.MyInstance.DismissSpirits(false);
        Debug.Log("Spirits Dismissed L");

        // Loss scene
        lossScene = gameInfo.RespawnLocation;
        //Debug.Log("--- LOSS SCENE: " + lossScene);
        // Will Need to Call the SceneDataManager to transition to save data.
        SceneDataManager.MyInstance.SceneTransition(lossScene);
        Debug.Log("Scene Transition L");
    }

}
