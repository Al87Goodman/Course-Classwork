//REFERENCE: inScope Studios - https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{ 
    [SerializeField]
    private GameObject slotPrefab = null;

    // Used to Hide/Show UI Element
    private CanvasGroup canvasGroup;
    
    // List of al the sltos that belong to a particular bag
    private List<SlotScript> slots = new List<SlotScript>();

    // Which Bag
    public int MyBagIndex { get; set; }

    // IF a bag is open or closed
    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    public List<SlotScript> MySlots 
    { 
        get
        {
            return slots;
        }
    }

    // Property: How many empty slots
    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (SlotScript slot in MySlots)
            {
                if (slot.IsEmpty)
                {
                    count++;
                }
            }
            return count;
        }
    }


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // return a list of items in a bag
    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();
        foreach (SlotScript slot in slots)
        {
            if (!slot.IsEmpty)
            {
                foreach (Item item in slot.MyItems)
                {
                    items.Add(item);
                }
            }
        }
        return items;
    }


    // Creates slots for this bag
    public void AddSlots(int slotCount)
    {
        for (int i=0; i< slotCount; i++)
        {
            SlotScript slot =  Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.MyIndex = i;
            slot.MyBag = this; // slot knows what bag it belongs to
            MySlots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        // All Slots Belonging to This Bag.
        foreach (SlotScript slot in MySlots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);
                return true;
            }
        }
        // unsuccessful add (can indicate Bag is Full)
        return false;
    }


    // Open/Close Inventory Bag Display
    public void OpenClose()
    {
        // Hidden set to 1, Shown set to 0
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts = true;  //  ? false : true; --> Caused Items to be unclickable.
    }
}
