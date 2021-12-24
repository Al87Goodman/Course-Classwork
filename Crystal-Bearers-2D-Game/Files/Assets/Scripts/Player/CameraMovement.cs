using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public Transform player;
	public float smoothness;
	public Vector2 max;
	public Vector2 min;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
    	//keeps camera from falling through map
    	Vector3 playerPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        if(transform.position != player.position){
        	//keep camera inside bounds
        	playerPosition.x = Mathf.Clamp(playerPosition.x, min.x, max.x);
        	playerPosition.y = Mathf.Clamp(playerPosition.y, min.y, max.y);
        	//move to player
        	transform.position = Vector3.Lerp(transform.position, playerPosition, smoothness);
        }
    }
}
