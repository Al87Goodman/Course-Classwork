//REFERENCE: inScope Studios - https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    //Holds the Varying Number of Items. Some Items will be stackable
    // Created a separate script for automatically handling stack events located in UIRelated
    private ObservableStack<Item> items = new ObservableStack<Item>();

    // Reference to Slot's Icon
    [SerializeField]
    private Image icon;

    // Refernce to Slot's Stack Size
    [SerializeField]
    private Text stackSize;

    // Reference to the Bag that the slot belongs to
   public BagScript MyBag { get; set; }

    // Slot Index (assigned in the bagscript)
    public int MyIndex { get; set; }

    // Checks to see if slot is empty... yeah name implies it ha
    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    // Maxed Out Stack or Room for More
    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }
            return true;
        }
    }

    // Get an Item
    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                // See What Item is On Top (if at all) - Peek is Stack Function
                return MyItems.Peek();
            }
            return null;
        }
    }


    public Image MyIcon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }
    
    // Count of Items on the Slot
    public int MyCount
    {
        get { return MyItems.Count; }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    // Get Items 
    public ObservableStack<Item> MyItems 
    { 
        get => items;
    }



    // Everytime these events are raised, called the updatestack function
    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    // Add Item to Slot
    // Returns true if item was added
    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = UnityEngine.Color.white;
        item.MySlot = this;

        return true;
    }

    // Check if slot is empty or item is of same type
    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;
            //Add Items to the same Slot
            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }
            return true;
        }
        return false;
    }

    // Remove Item from Slot
    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
        }
    }

    // Clear Items from outside scripts
    public void Clear()
    {
        if (MyItems.Count > 0)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            MyItems.Clear();
        }
    }

    // For Click Functionality of Items -- its big, sorry...
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(InventoryScript.MyInstance.FromSlot == null && !IsEmpty) // if nothing to move yet
            {
                // Have something in Hand and that something is a bag
                if (HandScript.MyInstance.MyMoveable != null)
                {
                    // check if item is a bag (only can swap bag with another bag)
                    if (MyItem is Bag && HandScript.MyInstance.MyMoveable is Bag)
                    {
                        InventoryScript.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag);
                    }
                }
                else
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.MyInstance.FromSlot = this;
                }
            }
            // CLick on slot that is null, empty, and carrying a bag around then dequip!
            else if(InventoryScript.MyInstance.FromSlot == null && IsEmpty && (HandScript.MyInstance.MyMoveable is Bag))
            {
                // Dequips bag from inventory
                Bag bag = (Bag)HandScript.MyInstance.MyMoveable;
                if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.MySlotCount > 0)
                {
                    AddItem(bag);
                    bag.MyBagButton.RemoveBag();  // remove bag
                    HandScript.MyInstance.Drop(); // drop from hand
                }
            }
            else if (InventoryScript.MyInstance.FromSlot != null) // if we have something to move
            {
                // Put Item Back To Original Slot -OR- Merge -OR- Try to Swap -OR- Try to Add
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) ||SwapItems(InventoryScript.MyInstance.FromSlot) ||AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;  // reset 
                }
            }
        }

        // Right Click to use Item (or sometimes just to discard).
        // NOTE: Need to implement a warning about discarding item..
        if (eventData.button == PointerEventData.InputButton.Right && !IsEmpty)
        {
            ItemUseManager.MyInstance.OpenItemOptionMenu(transform.position, MyItem);
        }
    }

    public void UseItem()
    {
        // If Item is Useable
        if(MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
    }

    public void TossItem()
    {
        // If Item is Tossable
        if (MyItem is ITossable)
        {
            (MyItem as ITossable).Toss();
        }
    }

    public bool StackItem(Item item)
    {
        if(!IsEmpty && item.MyTitle == MyItem.MyTitle && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }
        return false;
    }

    private bool PutItemBack()
    {
        // put back in same slot
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = UnityEngine.Color.white;
            return true;
        }
        return false;
    }


    private bool SwapItems(SlotScript from)
    {
        if(IsEmpty)
        {
            return false;
        }
        // If different Item Type OR if the count is larger than total stack size then swap.
        // (Swap Stacks...)
        if(from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            // Copy all the items to swap from Slot A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            // Clear Slot A & Add Slot B's Items to Slot A
            from.MyItems.Clear();
            from.AddItems(MyItems);

            // Clear Slot B & Move Slot A's Items to Slot B
            MyItems.Clear();
            AddItems(tmpFrom);

            return true;
        }
        return false;
    }

    // Merging Items Together
    private bool MergeItems(SlotScript from)
    {
        // If it is empty then no need to merge so return false
        if (IsEmpty)
        {
            return false;
        }
        // Check to see if Items are Same Type & if Item Slot Stacksize is Not Full
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            // number of available slots 
            int freeSlots = MyItem.MyStackSize - MyCount;

            // Add Items to the desired stack by popping from the selected(old) stack
            for (int i = 0; i < freeSlots; i++)
            {
                if (from.MyCount > 0)
                {
                    AddItem(from.MyItems.Pop());
                }
            }
            return true;
        }
        return false;
    }

    // Updates the Slot
    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty & MyBag.IsOpen)
        {
            UIManager.MyInstance.ShowToolTip(transform.position, MyItem);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideToolTip();
    }
}
