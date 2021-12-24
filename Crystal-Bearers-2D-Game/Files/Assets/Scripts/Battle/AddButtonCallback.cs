/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
 * Class to add attack functionality to menu button
 ***************************************************/
public class AddButtonCallback : MonoBehaviour
{
    [SerializeField]
    private bool physical;

    // Start is called before the first frame update
    void Start()
    {
        //add button listener at start up
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => addCallback());
    }

    //Method with functionality
    private void addCallback()
    {
        //get party object
        GameObject playerParty = GameObject.Find("Player Party");
        //get attack method
        playerParty.GetComponent<SelectUnit>().selectAttack(this.physical);
    }
}
