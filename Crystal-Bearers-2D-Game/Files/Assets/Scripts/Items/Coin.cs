using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour
{
	public int coinsToAdd;
	public GameObject inventory;
	public Signal coinSignal;
    // Start is called before the first frame update
    void Start()
    {
    	
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //triggers the even when collided with
    private void OnTriggerEnter2D(Collider2D player)
    {
    	if (player.CompareTag("Player"))
    	{
    		inventory.GetComponent<InventoryScript>().AddCoins(coinsToAdd);
    		Destroy(this.gameObject);
    		coinSignal.Raise();
    		//print (inventory.GetComponent<InventoryScript>().GetCoins());
    	}
    }
}
