using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Potion, Bag, Currency}

// Items cannot exist on their own.
public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    // Icon of Item (Moving & Placing)
    [SerializeField] private Sprite icon = null;

    // Number of Items in the Stack
    [SerializeField] private int stackSize = 0;

    // Name of the Item
    [SerializeField] private string title;
    
    // Enum Used for Color Display
    [SerializeField] private ItemType itemType;

    // References the slot that the item is in
    private SlotScript slot;

    // Property for Accessing the Icon
    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    // Property for Accessing the Stacksize
    public int MyStackSize 
    { 
        get
        {
            return stackSize;
        } 
        set
        {
            stackSize = value;
        }
    }

    public SlotScript MySlot
    {
        get
        {
            return slot;
        }
        set
        {
            slot = value;
        }
    }

    public string MyTitle
    {
        get
        {
            return title;
        }
    }

    // IDescribable and sets color depending on What Type of Item it is (Field in Inspector)
    public virtual string GetDescription()
    {
        string color = string.Empty;

        switch (itemType)
        {
            case ItemType.Potion:
                color = "purple";
                break;
            case ItemType.Bag:
                color = "blue";
                break;
            case ItemType.Currency:
                color = "#FFE939";
                break;
            default:
                break;
        }

        return string.Format("<color={0}>{1}</color>",color,title);
    }

    // Remove Itself from Inventory
    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
            MySlot = null; // remove reference
        }
    }
}
