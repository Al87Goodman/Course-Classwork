/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**************************************************************
 * Abstract class to display dynamic health and mana bars.
 * Will be used by particular children for particular stat.
 * ************************************************************/
public abstract class ShowUnitStat : MonoBehaviour
{
    //the current unit
    [SerializeField]
    protected GameObject unit;

    //get max value of stat we are displaying
    [SerializeField]
    private float maxValue;

    //scale for bar
    private Vector2 initialScale;

    void Start()
    {
        this.initialScale = this.gameObject.transform.localScale;
    }

    void Update()
    {
        if(this.unit)
        {
            //Set Values Based on Unit Stats

            //get current stat
            float newValue = this.curStat();
            //find percentage of current stat to max of stat
            float newScale = (this.initialScale.x * newValue) / this.maxStat();
            //adjust bar length based on percentage
            this.gameObject.transform.localScale = new Vector2(newScale, this.initialScale.y);
        }
    }

    //method to switch which unit is current one (and to use stats from)
    public void changeUnit(GameObject newUnit)
    {
        this.unit = newUnit;
    }

    abstract protected float curStat();
    abstract protected float maxStat();
}
