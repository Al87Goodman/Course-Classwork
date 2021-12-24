/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************************************
 * Class to destroy damage text after displayed
 ***********************************************/
public class KillDamageText : MonoBehaviour
{

    [SerializeField]
    private float destroyTime;

    void Start()
    {
        Destroy(this.gameObject, this.destroyTime);
    }

    void OnDestroy()
    {
        GameObject turnSystem = GameObject.Find("TurnSystem");
        turnSystem.GetComponent<TurnSystem>().nextTurn();
    }
}
