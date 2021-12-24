/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/***********************************************************
 * Class to handle experience and other rewards of battle
 **********************************************************/
public class CollectReward : MonoBehaviour
{
    [SerializeField]
    private int experience;
    public GameObject inventory;
    private int val;
    private int coins;
    public List<CrystalItem> playerCrystals;
    private string sceneName;

    void Start()
    {
        GameObject turnSystem = GameObject.Find("TurnSystem");
        turnSystem.GetComponent<TurnSystem>().enemyEncounter = this.gameObject;
    }

    //Method to give rewards
    public void collectReward()
    {
        //find all alive party members, and give experience divided equally among them
        GameObject[] livingParty = GameObject.FindGameObjectsWithTag("PartyUnit");
        int experiencePerUnit = this.experience / livingParty.Length;

        foreach (GameObject partyUnit in livingParty)
        {
            partyUnit.GetComponent<UnitStats>().receiveExperience(experiencePerUnit);
        }

        //runs collectLoot function to add an item to inventory
       collectLoot();

        //destroy object once complete
        Destroy(this.gameObject);
    }

    //uses a random int to choose case in switch statement to then put specified item into inventory. also random amount of coins
    public void collectLoot()
    {
    	val = Random.Range(1,5);
    	coins = Random.Range(1,11);
    	inventory = GameObject.FindGameObjectWithTag("Inventory");

    	switch (val)
    	{
    		case 1:
    			Debug.Log("give coins " + coins);
    			inventory.GetComponent<InventoryScript>().AddCoins(coins);
    			break;
    		case 2:
    		Debug.Log("give health pot");
    			inventory.GetComponent<InventoryScript>().AddHealth();
    			break;
    		case 3:
    		Debug.Log("give magic pot");
    			inventory.GetComponent<InventoryScript>().AddMagic();
    			break;
    		case 4:
    		Debug.Log("give poison pot");
    			inventory.GetComponent<InventoryScript>().AddPoison();
    			break;


    	}

    }

    public void HalveCoins()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        coins = inventory.GetComponent<InventoryScript>().GetCoins();
        inventory.GetComponent<InventoryScript>().RemoveCoins(coins/2);
    }

    // Collect The Respective Crystal 
    public void CollectCrystal()
    {
        sceneName = SceneManager.GetActiveScene().name;
        string[] trainerCrystals = { "Sonic", "Ham-Ham","Spike","Stabagator","Boom"};
        int randIndex = Random.Range(0, 5);
        //Debug.Log("--- SCENE NAME: " + sceneName);         

        switch (sceneName)
        {
            case "Arborius_Boss":
                PlayerCrystalScript.MyInstance.AddCrystalByName("Gordon Antsy");
                break;
            case "Hagitha_Boss":
                PlayerCrystalScript.MyInstance.AddCrystalByName("Ironwood");
                break;
            case "Marina_Boss":
                PlayerCrystalScript.MyInstance.AddCrystalByName("Barnacle Bruh");
                break;
            case "Toxitra_Boss":
                PlayerCrystalScript.MyInstance.AddCrystalByName("Fred");
                break;
            case "Ruby_Boss":
                PlayerCrystalScript.MyInstance.AddCrystalByName("Birdie The Kid");
                break;
            case "Sample_Battle":
                PlayerCrystalScript.MyInstance.AddCrystalByName(trainerCrystals[randIndex]);
                break;
            default:
                //Debug.Log("No Crystal Awarded");
                break;

        }


    }
}
