using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewCrystal", menuName = "Inventory/Crystals")]
public class CrystalItem : ScriptableObject
{
    // Attributes of Each Crystal
    public int crystalID; // unique to each crystal type.
    public string spiritObject;
    public string crystalName;
    public string crystalDescription;
    public Sprite crystalImage;
    public int numberHeld;
    public bool useable;
    public bool unique;
    public bool isCrystal = true;
    public bool isActive; // ready to be used or in storage
    public UnityEvent thisEvent;

    // Invoked Crystals
    public void Use()
    {
        // Maybe Use to Start Summoning Spirit
        Debug.Log("Using Crystal");
        thisEvent.Invoke();
    }

    // Crystal Bag Buttons Clicked On (Activate & Store)
    public void ActivatePressed()
    {
        //Debug.Log("Activate Crystal!");
        isActive = true;
        //thisEvent.Invoke();
    }
    public void StorePressed()
    {
        //Debug.Log("Store Crystal!");
        isActive = false;
        //thisEvent.Invoke();
    }

    public void DiscardPressed()
    {
        //Debug.Log("Discard Crystal!");
        //thisEvent.Invoke();
        if (numberHeld > 0)
        {
            DecreaseAmount(1);
        }

    }

    public void DecreaseAmount(int amountDecrease)
    {
        numberHeld -= amountDecrease;
        if (numberHeld < 0)
        {
            numberHeld = 0;
        }
    }
    

}
