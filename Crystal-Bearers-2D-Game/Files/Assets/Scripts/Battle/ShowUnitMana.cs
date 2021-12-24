/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnitMana : ShowUnitStat
{
    override protected float curStat()
    {
        return unit.GetComponent<UnitStats>().MP;
    }

    override protected float maxStat()
    {
        return unit.GetComponent<UnitStats>().maxMP;
    }
}
