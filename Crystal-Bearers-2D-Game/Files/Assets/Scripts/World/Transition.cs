using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
	public string destinationScene;
	public Vector2 playerPosition;
	public CharacterPosition storedPosition;
    public GameInfo gameInfo;

    // Gets Called from the New Game Button from the Main Menu
    public void OnNewGame()
    {
        Debug.Log("*** NEW GAME");
        // Set the Scriptable Object to True. This is referenced in CharacterPosition script and Panel
        gameInfo.NewGame = true;
        gameInfo.RespawnLocation = "BasicTown";
        gameInfo.FleeLocation = "BasicTown";
        storedPosition.startPosition = playerPosition;
        SceneManager.LoadScene("BasicTown");
    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        if(player.CompareTag("Player")){
            storedPosition.startPosition = playerPosition;

            try
            {
                if (SceneManager.GetActiveScene().name == "Wild")
                {
                    //Debug.Log("TRYING TO STORE DESTINATION SCENE: " + destinationScene);
                    gameInfo.RespawnLocation = destinationScene;
                    gameInfo.FleeLocation = destinationScene;
                }
                else if (destinationScene == "Wild")
                {
                    gameInfo.FleeLocation = destinationScene;
                    //Debug.Log("FLEE LOCATION CHANGE");
                }

            }
            catch (System.Exception)
            {

                //throw;
            }
            
            //Saves Scene Data, Loads Scence, Loads Previously Saved Data
            SceneDataManager.MyInstance.SceneTransition(destinationScene);

        }
    }
}
