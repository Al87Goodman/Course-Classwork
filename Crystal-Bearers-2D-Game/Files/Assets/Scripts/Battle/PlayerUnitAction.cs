/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/********************************************
 * Class to handle a party member's attacks
 *******************************************/
public class PlayerUnitAction : MonoBehaviour
{
    [SerializeField]
    private GameObject physicalAttack;

    [SerializeField]
    private GameObject magicalAttack;

    private GameObject currentAttack;

    void Awake()
    {
        this.physicalAttack = Instantiate(this.physicalAttack, this.transform) as GameObject;
        this.magicalAttack = Instantiate(this.magicalAttack, this.transform) as GameObject;

        this.physicalAttack.GetComponent<AttackTarget>().owner = this.gameObject;
        this.magicalAttack.GetComponent<AttackTarget>().owner = this.gameObject;

        //default attack is physical
        this.currentAttack = this.physicalAttack;
    }

    //Method to select type of attack
    public void selectAttack(bool isPhysical)
    {
        this.currentAttack = (isPhysical) ? this.physicalAttack : this.magicalAttack;
    }

    //Method to execute attack
    public void act(GameObject target)
    {
        this.currentAttack.GetComponent<AttackTarget>().hit(target);
    }

    public void updateHUD()
    {
        //update name
        GameObject playerUnitName = GameObject.Find("NameText") as GameObject;
        playerUnitName.GetComponent<ShowUnitName>().changeUnit(this.gameObject);

        //update health bar
        GameObject playerUnitHealthBar = GameObject.Find("HealthBar") as GameObject;
        playerUnitHealthBar.GetComponent<ShowUnitHealth>().changeUnit(this.gameObject);

        //update mana bar
        GameObject playerUnitManaBar = GameObject.Find("ManaBar") as GameObject;
        playerUnitManaBar.GetComponent<ShowUnitMana>().changeUnit(this.gameObject);
    }
}
