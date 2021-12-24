using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vendor : MonoBehaviour
{
	public Button selectHealth;
	public Button selectMagic;
	public Button selectPoison;
	public GameObject player;
	public GameObject vendorBar;
	private bool playerInRange;
	private bool shopOpen;
	public Text vendorText;
	public string vendorDialog;
	public GameObject dialogBox;
	public GameObject inventory;
	public Signal coinSig;

	public void Start()
	{
		selectHealth.onClick.AddListener(BuyHealth);
		selectMagic.onClick.AddListener(BuyMagic);
		selectPoison.onClick.AddListener(BuyBag);

	}


    public void Update()
    {
    	if(Input.GetKeyDown(KeyCode.Space) && playerInRange){
    		if(dialogBox.activeInHierarchy){
    			dialogBox.SetActive(false);
    			vendorBar.SetActive(false);
    		}
    		else{
    			dialogBox.SetActive(true);
    			vendorBar.SetActive(true);
    			vendorText.text = vendorDialog;
    		}
    	} 
    		
    }
    private void BuyHealth(){
    	if (inventory.GetComponent<InventoryScript>().RemoveCoins(5)){
    		inventory.GetComponent<InventoryScript>().AddHealth();
    		coinSig.Raise();
    	}
    }
	private void BuyMagic(){
		if (inventory.GetComponent<InventoryScript>().RemoveCoins(5)){
			inventory.GetComponent<InventoryScript>().AddMagic();
			coinSig.Raise();
		}
	}
    private void BuyBag(){
		if (inventory.GetComponent<InventoryScript>().RemoveCoins(10)){
            if (inventory.GetComponent<InventoryScript>().MyBags.Count > 0)
            {
                inventory.GetComponent<InventoryScript>().AddBag();
                coinSig.Raise();
            }
            else
            {
                inventory.GetComponent<InventoryScript>().NewBag();
                coinSig.Raise();
            }
		}
	}		


    

    private void OnTriggerEnter2D(Collider2D player){
        if (player.CompareTag("Player")){
        	playerInRange= true;
        }
    }
    private void OnTriggerExit2D(Collider2D player){
        if(player.CompareTag("Player")){
        	playerInRange = false;
        	dialogBox.SetActive(false);
        	vendorBar.SetActive(false);
        }
    }
}
