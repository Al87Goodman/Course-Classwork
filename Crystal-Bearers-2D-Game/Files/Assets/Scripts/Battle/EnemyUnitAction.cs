/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************
 * Class to handle the attack of enemy units
 ********************************************/
public class EnemyUnitAction : MonoBehaviour
{
    [SerializeField]
    private GameObject attack;

    [SerializeField]
    private string targetsTag;

    void Awake()
    {
        this.attack = Instantiate(this.attack);
        this.attack.GetComponent<AttackTarget>().owner = this.gameObject;
    }

    //Method to choose a random target from all present party members
    GameObject findRandomTarget()
    {
        //make array of possible targets
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetsTag);

        if(possibleTargets.Length > 0)
        {
            //choose random target
            int targetIndex = Random.Range(0, possibleTargets.Length);
            GameObject target = possibleTargets[targetIndex];

            return target;
        }
        return null;

    }

    //Method to execute actual attack
    public void act()
    {
        GameObject target = findRandomTarget();
        this.attack.GetComponent<AttackTarget>().hit(target);
    }
}
