/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnitHealth : ShowUnitStat
{

    override protected float curStat()
    {
        return unit.GetComponent<UnitStats>().HP;
    }

    override protected float maxStat()
    {
        return unit.GetComponent<UnitStats>().maxHP;
    }
}
