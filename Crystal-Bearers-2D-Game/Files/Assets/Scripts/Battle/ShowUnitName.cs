using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/****************************************************************
 * Class to change battle menu name to current unit's name
 ***************************************************************/
public class ShowUnitName : MonoBehaviour
{
    //the text field
    public Text name;

    //the current unit
    [SerializeField]
    protected GameObject unit;

    void Start()
    {
        //get the text component of NameText object
        name = GetComponent<Text>();
    }

    void Update()
    {
        if (this.unit)
        {
            //set name based on unit name
            name.text = unit.GetComponent<UnitStats>().spiritName;
        }
    }

    public void changeUnit(GameObject newUnit)
    {
        this.unit = newUnit;
    }
}