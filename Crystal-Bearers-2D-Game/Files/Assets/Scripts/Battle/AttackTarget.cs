/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************************************************************
 * Class to calculate damage of attacks and handle attacking
 **************************************************************/
public class AttackTarget : MonoBehaviour
{
    //used to tie this particular instance of attack script to particular attacker object
    public GameObject owner;

    //attack's random number generator
    protected System.Random rand = new System.Random();

    /*
    [SerializeField]
    private string attackAnimation;
    */

    //denotes if it is a magic attack or normal
    [SerializeField]
    private bool magicAttack;

    //MP cost of magic attack
    [SerializeField]
    private int manaCost;

    //stores attack and defense value multipliers
    [SerializeField]
    private float minAttackMulti;

    [SerializeField]
    private float maxAttackMulti;

    [SerializeField]
    private float minDefenseMulti;

    [SerializeField]
    private float maxDefenseMulti;

    //Method to actually calculate attack and damage
    public void hit(GameObject target)
    {
        //get stats of attacker
        UnitStats ownerStats = this.owner.GetComponent<UnitStats>();

        //get stats of defender
        UnitStats targetStats = target.GetComponent<UnitStats>();

        //check enough MP for attack
        if(ownerStats.MP >= this.manaCost)
        {
            //attack strength
            double attackMulti = (rand.NextDouble() * (this.maxAttackMulti - this.minAttackMulti)) + this.minAttackMulti;
            //attack damage
            double damage = (this.magicAttack) ? attackMulti * ownerStats.magic : attackMulti * ownerStats.attack;

            //opponent's defense strength
            double defenseMulti = (rand.NextDouble() * (this.maxDefenseMulti - this.minDefenseMulti)) + this.minDefenseMulti;
            //final damage with defense
            damage = System.Math.Max(1, damage - (defenseMulti * targetStats.defense));

            //play attack animation
            //this.owner.GetComponent<Animator>().Play(this.attackAnimation);

            //apply damage to opponent
            targetStats.recieveDamage((int)damage);

            //take away MP for attack
            ownerStats.MP -= this.manaCost;

        }
    }

}
