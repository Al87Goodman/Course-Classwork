/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/***************************************
 * Class to handle scene transitions
 **************************************/
public class ChangeScene : MonoBehaviour
{
    public void loadNextScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
