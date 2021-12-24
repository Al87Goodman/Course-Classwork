//REFERENCE: inScope Studios - https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag",menuName ="Items/Bag",order =1)]
public class Bag : Item, IUseable
{
    // Number of Slots in a bag
    [SerializeField]
    private int slots;
    
    [SerializeField]
    private GameObject bagPrefab = null;

    // Reference to bagScript...
    public BagScript MyBagScript { get; set; }

    // Reference to BagButton...
    public BagButton MyBagButton { get; set; }

    // Property for Getting Slots.
    public int MySlotCount { get => slots; }
    public void Intialize(int slots)
    {
        this.slots = slots;
    }

    // Equips the bag!
    public void Use()
    {
        // Check to Make Sure the # of Allowed Bags is Not Full.
        if (InventoryScript.MyInstance.CanAddBag)
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);

            // If Bag does Not have a bag button set already - first empty bag button
            if (MyBagButton == null)
            {
                InventoryScript.MyInstance.AddBag(this);
            }
            // specific bag bag
            else
            {
                InventoryScript.MyInstance.AddBag(this,MyBagButton); // overloaded function
            }

            // Used to determine Where to put the items
            MyBagScript.MyBagIndex = MyBagButton.MyBagIndex;
        }
    }

    // Assigned a refernce to a bagscript and create a bag that has empty slots for placing items
    public void SetupScript()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlots(slots);
    }

    // Overrides Item GetDescription()
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n{0} slot bag", slots);
    }
}


