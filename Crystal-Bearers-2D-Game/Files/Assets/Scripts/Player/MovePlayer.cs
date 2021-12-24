//script created using the assistance of:
//https://www.youtube.com/watch?v=--N5IgSUQWI&list=PL4vbr3u7UKWp0iM1WIfRjCDTI03u43Zfu&index=3
//-camerwil
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
	public float movementSpeed;
	private Rigidbody2D playerRigidbody;
	private Vector3 movement;
	public CharacterPosition start;
	private Animator animation;
    //coin signal so that coins update when player is loaded
    //public Signal coinSig;

    // Singleton -- Added to Get Access in SaveManager (SORRY..)
    private static MovePlayer instance;
    public static MovePlayer MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MovePlayer>();
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        //coinSig.Raise();
        //transform.position = start.startPosition; //commented out due to issues with loading saved file player position.
        animation = GetComponent<Animator>();
    }
    // Used in SaveManager.
    public void DefaultValues()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        transform.position = start.startPosition;
        //Debug.Log("Default Position - X: " + transform.position.x + " Y: " + transform.position.y);
        animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     movement = Vector3.zero;
     movement.x = Input.GetAxisRaw("Horizontal");
     movement.y = Input.GetAxisRaw("Vertical");
     if(movement != Vector3.zero)
     {
     	Move();
     	animation.SetFloat("moveX", movement.x);
     	animation.SetFloat("moveY", movement.y);
     	animation.SetBool("moving", true);
     }
     else{
     	animation.SetBool("moving", false);
     }

    }

    void Move()
    {
        playerRigidbody.MovePosition(transform.position + movement.normalized * movementSpeed * Time.deltaTime);
    }
}
