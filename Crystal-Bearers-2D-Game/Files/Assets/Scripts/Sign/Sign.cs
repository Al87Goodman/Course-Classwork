using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sign : MonoBehaviour
{
	public GameObject dialogBox;
	public Text signText;
	public string sign;
	public bool signActive;

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
    			signText.text = sign;
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

