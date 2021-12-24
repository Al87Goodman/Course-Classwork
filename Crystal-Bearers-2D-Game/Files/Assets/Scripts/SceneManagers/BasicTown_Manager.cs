using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTown_Manager : MonoBehaviour
{
    // Singleton 
    private static BasicTown_Manager instance;
    public static BasicTown_Manager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BasicTown_Manager>();
            }

            return instance;
        }
    }


    // Data to Persist
    [SerializeField] private Coin[] coins;
    [SerializeField] private GameObject[] npcs = null;




    private void Awake()
    {
        // Load Game Information
        // Load Game Information

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
