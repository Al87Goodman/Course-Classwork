using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
	public TextMeshProUGUI textBox;
	private int coins;
	public GameObject inventory;

	void OnRenderObject(){
		coins = inventory.GetComponent<InventoryScript>().GetCoins();
    	textBox.text = "" + coins;

	}
        
    
    public void ChangeCoins(){
    	coins = inventory.GetComponent<InventoryScript>().GetCoins();
    	textBox.text = "" + coins;
    	

    }
}
