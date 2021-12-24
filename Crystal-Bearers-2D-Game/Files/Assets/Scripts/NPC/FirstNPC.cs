using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FirstNPC : MonoBehaviour
{
	public GameObject dialogBox;
	public GameObject inventory;
	public Text signText;
	public string sign1;
	public string sign2;
	public bool signActive;
	public BoolValue visited;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.Space) && signActive){
    		if(dialogBox.activeInHierarchy){
    			dialogBox.SetActive(false);
    		}
    		else{
    			dialogBox.SetActive(true);
    			
    			if (visited.RuntimeValue == false)
	        	{
	        		visited.RuntimeValue = true;
	        		inventory.GetComponent<InventoryScript>().NewBag();
	        		signText.text = sign1;
	        	}else{
	        		signText.text = sign2;
	        	}
    		}
    	}
    }
    private void OnTriggerEnter2D(Collider2D player){
        if (player.CompareTag("Player")){
        	signActive = true;
        	
        }
    }
    private void OnTriggerExit2D(Collider2D player){
        if(player.CompareTag("Player")){
        	signActive = false;
        	dialogBox.SetActive(false);
        }
    }
    }

