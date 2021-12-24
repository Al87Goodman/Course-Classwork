/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/****************************************
 * Class for a unit's stats, as well as
 * a few methods related to them.
 ***************************************/
public class UnitStats : MonoBehaviour, IComparable
{
    //fighter's random generator used for randomization
    protected System.Random rand = new System.Random();

    /*
    //sprite animator
    [SerializeField]
    private Animator animator;
    */

    //used for damage display
    [SerializeField]
    private GameObject damageTextPrefab;
    [SerializeField]
    private Vector2 damageTextPosition;

    //identifier
    public int ID;
    public string spiritName;
    public string typeTag;

    public string getTag()
    {
        return typeTag;
    }

    //fighter's stats
    public int HP; //current HP
    public int maxHP;
    public int MP; //current MP
    public int maxMP;
    public int attack;
    public int magic;
    public int defense;
    public int magDef;
    public int speed;

    //fighter's stat increase bases
    //used to determine how much each stat increases at level up
    //can increase between 0x and 2x amount
    [SerializeField]
    private int incHP;
    [SerializeField]
    private int incMP;
    [SerializeField]
    private int incAttack;
    [SerializeField]
    private int incMagic;
    [SerializeField]
    private int incDefense;
    [SerializeField]
    private int incMagDef;
    [SerializeField]
    private int incSpeed;


    //fighter's level
    [SerializeField]
    private int level;

    //experience total
    [SerializeField]
    private int totalExp;

    //experience needed for next level up
    [SerializeField]
    private int nextLevel;

    // Used for retrieving private data when creating initial spirits in SpiritDataScript
    public int MyLevel { get => level; set => level = value; }
    public int MyNextLevel { get => nextLevel; set => nextLevel = value; }
    public int MyTotalExp { get => totalExp; set => totalExp = value; }

    /************************
     * Turn Functionality
     * **********************/
    //location in turn order of next turn
    public int nextActTurn;

    //flag to mark as dead or not
    private bool dead = false;


    //Method to calculate next turn value
    public void calculateNextActTurn(int currentTurn)
    {
        //set next turn based on speed stat
        this.nextActTurn = currentTurn + (int)System.Math.Ceiling(100.0f / this.speed);
    }

    public int CompareTo(object otherStats)
    {
        return nextActTurn.CompareTo(((UnitStats)otherStats).nextActTurn);
    }

    //Method to check death status
    public bool isDead()
    {
        return this.dead;
    }

    //Method to process damage
    public void recieveDamage(double damage)
    {
        this.HP = (int)(this.HP - damage);
        //animator.Play("Hit");

        /***********************
        * Display Damage Amount
        ************************/
        //find the Battle Message and add damage text to it
        GameObject battleMessage = GameObject.Find("Battle Message");
        battleMessage.GetComponent<Text>().text = this.spiritName + " takes " + damage + " damage";

        //rough way for delaying text
        GameObject HUDCanvas = GameObject.Find("HUDCanvas");
        GameObject damageText = Instantiate(this.damageTextPrefab, HUDCanvas.transform) as GameObject;
        damageText.transform.localPosition = this.damageTextPosition;
        damageText.transform.localScale = new Vector2(1.0f, 1.0f);

        //check if unit is dead
        if (this.HP <= 0)
        {
            this.dead = true;
            this.HP = 0;

            //message the death
            GameObject deathMessage = GameObject.Find("Battle Message");
            deathMessage.GetComponent<Text>().text = this.spiritName + " has died";

            //rough way for delaying text
            GameObject damageText2 = Instantiate(this.damageTextPrefab, HUDCanvas.transform) as GameObject;
            damageText2.transform.localPosition = this.damageTextPosition;
            damageText2.transform.localScale = new Vector2(1.0f, 1.0f);

            this.tag = "DeadUnit";
            Debug.Log("Set Dead");

            if(this.typeTag == "EnemyUnit")
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
            
        }

        GameObject turnSystem = GameObject.Find("TurnSystem");
        turnSystem.GetComponent<TurnSystem>().nextTurn();
    }

    //Method to change experience total
    public void receiveExperience(int experience)
    {
        this.totalExp = totalExp + experience;

        this.nextActTurn = 0;

        //check if fighter levels up
        while(totalExp >= nextLevel)
        {
            this.levelUp();
        }
    }

    //Method to level up fighter
    private void levelUp()
    {
        //add to stats, between 0x to 2x base stat increase
        this.maxHP = this.maxHP + (int)((rand.NextDouble() * 2) * incHP);
        this.maxMP = this.maxMP + (int)((rand.NextDouble() * 2) * incMP);
        this.attack = this.attack + (int)((rand.NextDouble() * 2) * incAttack);
        this.magic = this.magic + (int)((rand.NextDouble() * 2) * incMagic);
        this.defense = this.defense + (int)((rand.NextDouble() * 2) * incDefense);
        this.magDef = this.magDef + (int)((rand.NextDouble() * 2) * incMagDef);
        this.speed = this.speed + (int)((rand.NextDouble() * 2) * incSpeed);

        //heal back to max HP and max MP
        this.HP = this.maxHP;
        this.MP = this.maxMP;

        //advance level
        this.level = this.level + 1;

        //set next level up experience amount
        this.nextLevel = nextLevel + (level * 100);

        /**************************
        * Display Level Up Message
        * (similar code to damage)
        ***************************/
        Debug.Log("Level Up");
        GameObject levelMessage = GameObject.Find("Battle Message");
        levelMessage.GetComponent<Text>().text = this.spiritName + " has leveled up!";
    }

    //Method to revive unit
    public void revive()
    {
        this.HP = this.maxHP;
        this.MP = this.maxMP;
    }
}
