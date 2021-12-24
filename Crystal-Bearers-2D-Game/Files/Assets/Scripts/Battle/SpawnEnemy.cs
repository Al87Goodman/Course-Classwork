/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/****************************************************
 * Class to spawn and start battle from overworld
 ***************************************************/
public class SpawnEnemy : MonoBehaviour
{
    //battle type to load
    [SerializeField]
    private string battleName;

    //scene to spawn on
    [SerializeField]
    private string sceneName;

    //possible encounters for spawner
    [SerializeField]
    private GameObject encounter1;
    [SerializeField]
    private GameObject encounter2;
    [SerializeField]
    private GameObject encounter3;

    //array to hold encounters
    private GameObject[] encounterSelector;

    //flag to tell class to start spawning enemy encounter
    private bool spawning = false;

    //preserve player position when returning from battle
    public Vector2 playerPosition;
    public CharacterPosition storedPosition;


    void Start()
    {

        if(SceneManager.GetActiveScene().name == sceneName)
        {
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject);
        }

        //populate array of possible encounters
        encounterSelector = new GameObject[] { encounter1, encounter2, encounter3 };
        
    }

   //Method to spawn enemy encounter
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //only spawn if in Battle scene
        if(scene.name == battleName)
        {
            //and only spawn if spawning is true
            if(this.spawning)
            {
                //randomly choose which encounter to spawn
                int enc = Random.Range(0, 3);

                Debug.Log("Chose encounter " + enc);

                //spawn enemy encounter
                Instantiate(encounterSelector[enc]);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
            //once encounter spawned, destroy this object, as job is done
            Destroy(this.gameObject);
        }
        else if(scene.name != battleName)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            Destroy(this.gameObject);
        }

    }

    //Method to detect when player collides with spawner
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            //save player position
            storedPosition.startPosition = playerPosition;
            //turn spawner on
            this.spawning = true;

            //Saves Scene Data, Loads Scene, Loads Previously Saved Data
            SceneDataManager.MyInstance.SceneTransition(battleName);
        }
    }
}
