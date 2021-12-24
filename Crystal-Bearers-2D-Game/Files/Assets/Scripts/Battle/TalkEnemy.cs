/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/***********************************************************
 * Class to spawn and start battle, with added dialogue
 **********************************************************/
public class TalkEnemy : MonoBehaviour
{
    //battle type to load
    [SerializeField]
    private string battleName;

    //scene to spawn on
    [SerializeField]
    private string sceneName;

    //encounter for spawner
    [SerializeField]
    private GameObject encounter;

    //flag to tell class to start spawning enemy encounter
    private bool spawning = false;

    //preserve player position when returning from battle
    public Vector2 playerPosition;
    public CharacterPosition storedPosition;

    public GameObject dialogBox;
    public Text signText;
    public string sign;
    public bool signActive;

    public bool goToBattle;


    void Start()
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject);
        }

        goToBattle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && signActive)
        {
            // Checks to see if any active spirits are able to battle
            if (SpiritDataScript.MyInstance.AbleToBattle())
            {
                if (dialogBox.activeInHierarchy)
                {
                    dialogBox.SetActive(false);


                }
                else
                {
                    dialogBox.SetActive(true);
                    signText.text = sign;
                }

                goToBattle = true;
            }
            //else display another dialog stating heal party or activate other crystals.

        }

        


    }

    //Method to spawn enemy encounter
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //only spawn if in Battle scene
        if (scene.name == battleName)
        {
            //and only spawn if spawning is true
            if (this.spawning)
            {
                //spawn enemy encounter
                Instantiate(encounter);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
            //once encounter spawned, destroy this object, as job is done
            Destroy(this.gameObject);
        }
        else if (scene.name != battleName)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            Destroy(this.gameObject);
        }

    }

    //Method to detect when player collides with spawner
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            signActive = true;
        }

        if (goToBattle == true)
        {
            //save player position
            //storedPosition.startPosition = playerPosition;
            //turn spawner on
            this.spawning = true;

            //Saves Scene Data, Loads Scene, Loads Previously Saved Data
            SceneDataManager.MyInstance.SceneTransition(battleName);
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            signActive = false;
            dialogBox.SetActive(false);
        }
    }
}
