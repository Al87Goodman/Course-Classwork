//REFERENCE: inScope Studios - https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;

    // Singleton Method
    private static InventoryScript instance;
    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }
    //Number of coins
    private int coins;
    // Click to move somewhere else
    private SlotScript fromSlot;

    // Hold the number of allowed bags
    private List<Bag> bags = new List<Bag>();

    //assigning a new bag button when it is equipped 
    [SerializeField]
    private BagButton[] bagButtons = null;

    [SerializeField]
    private Item[] items = null;

    // Count total empty slots in all bags
    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }
            return count;
        }
    }

    // Total Amount of Slots
    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;
            foreach(Bag bag in bags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }
            return count;
        }
    }

    // Full Slot Count
    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }

    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }
        set
        {
            // Grey Out Inventory Slot
            fromSlot = value;
            if(value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }

    public List<Bag> MyBags
    {
        get
        {
            return bags;
        }
    }

    // NOTE (Remove): For Debug Purposes Only

    private void Awake()
    {
        /*
        if (bags.Count < 1)
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Intialize(16);
            bag.Use();
        }
        */
    }

    // Max Number of Bags Allowed
    public bool CanAddBag
    {
        get
        { 
            return bags.Count < 4; 
        }
    }
    //returns the total coins
    public int GetCoins()
    {
    	return coins;
     }


    private void Update()
    {
        //NOTE: DEBUG ONLY - Add a New Bag 
        /*
        if (Input.GetKeyDown(KeyCode.J))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Intialize(16);
            bag.Use();
        }
        //NOTE: DEBUG ONLY - Add Iem to the Bag
        if (Input.GetKeyDown(KeyCode.K))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Intialize(16);
            AddItem(bag);
        }
        */

        //NOTE: DEBUG ONLY - Add Health Potion to the Bag
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            MagicPotion potionMagic = (MagicPotion)Instantiate(items[2]);
            AddItem(potionMagic);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PoisonPotion potionPoison = (PoisonPotion)Instantiate(items[3]);
            AddItem(potionPoison);
        }

    }
    //takes an int and adds it to the total coins
    public void AddCoins(int add)
    {
    	coins += add;
    }

    //takes an int and removes coins
    public bool RemoveCoins(int sub)
    {
    	if (sub <= coins)
    	{
    		coins -= sub;
    		return true;
    		}else{
    			return false;
    		}

    }
    // Equips a bag to Inventory
    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            // If No Bags are equipped then equip a bag on first empty slot
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                bags.Add(bag);
                bag.MyBagButton = bagButton;
                bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex); // set bag ranking hierarchy
                break;
            }
        }
    }

    // Overload 1  of AddBag Function!!
    // Takes a bag and places on a specific bag button
    public void AddBag(Bag bag, BagButton bagButton)
    {
        bags.Add(bag);
        bagButton.MyBag = bag;
        bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex); // set bag ranking hierarchy
    }

    // Overload 2 of AddBag Function (Used in SaveManager)
    public void AddBag(Bag bag, int bagIndex)
    {
        // Create a Bag with empty slots
        bag.SetupScript();
        // add bag to bags
        MyBags.Add(bag);
        // Assign bag a bag button
        bag.MyBagButton = bagButtons[bagIndex];
        // Bag Button points to the bag
        bagButtons[bagIndex].MyBag = bag;
    }
    // Take bag from bag list and removeand destroy it.
    public void RemoveBag(Bag bag)
    {
        bags.Remove(bag);
        Destroy(bag.MyBagScript.gameObject);
    }

    // Swap Bags Around
    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (MyTotalSlotCount - oldBag.MySlotCount) + newBag.MySlotCount;

        // Room for the swap!
        if (newSlotCount - MyFullSlotCount >= 0)
        {
            // do the ol'Swappity Swapp!
            List<Item> bagItems = oldBag.MyBagScript.GetItems();
            RemoveBag(oldBag);

            newBag.MyBagButton = oldBag.MyBagButton; // assign newbag to a bagbutton (use overloaded function)
            newBag.Use();

            // Make sure no duplicate bags
            foreach (Item item in bagItems)
            {
                if (item != newBag)
                {
                    AddItem(item);
                }
            }

            AddItem(oldBag);
            HandScript.MyInstance.Drop();
            MyInstance.fromSlot = null; // important! If not set to null, will be able to add mulitple bags..
        }
    }

    //Add an Item to the Inventory by looking through each bag
    public void AddItem(Item item)
    {
        if(item.MyStackSize > 0)
        {
            if(PlaceInStack(item))
            {
                return;
            }
        }
        PlaceInEmpty(item);

    }

    private void PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if(bag.MyBagScript.AddItem(item))
            {
                OnItemCountChanged(item);
                return;
            }
        }
    }

    // Place an Item on an Inventory Stack
    private bool PlaceInStack(Item item)
    {
        foreach(Bag bag in bags)
        {
            foreach(SlotScript slots in bag.MyBagScript.MySlots)
            {
                //If is stackable
                if(slots.StackItem(item))
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }
        return false;
    }

    // Opens and Closes All Da Bags.
    public void OpenClose()
    {
        // Checks to see if any bags are closed
        bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);

        // if closed bag == true, then open all closed bags
        // if closed bag == false, then clse all open bags
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.IsOpen != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    // Get only the slots that hold items
    public List<SlotScript> GetAllItems()
    {
        List<SlotScript> slots = new List<SlotScript>();
        // Look in All the Bags
        foreach (Bag bag in MyBags)
        {
            // Check All the Slots
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // If a slot is not empty, add it to the list
                if (!slot.IsEmpty)
                {
                    slots.Add(slot);
                }

            }
        }
        return slots;
    }

    // Place Item in specific slot in a specific bag
    public void PlaceInSpecific(Item item, int slotIndex, int bagIndex)
    {
        bags[bagIndex].MyBagScript.MySlots[slotIndex].AddItem(item);
    }


    // Look through inventory for passed in Item Type
    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();
        foreach (Bag bag in bags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // If same Type then put all that type into the useables stack
                if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                {
                    foreach (Item item in slot.MyItems)
                    {
                        useables.Push(item as IUseable);
                    }
                }
            }
        }
        return useables;
    }

    // Used in SaveMangaer
    public IUseable GetUseable(string type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();
        foreach (Bag bag in bags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // If same Type then put all that type into the useables stack
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    return (slot.MyItem as IUseable);
                }
            }
        }
        return null;
    }

    // Make sure someone is listening to event before calling (itemCountChangedEvent)
    // prevents null references
    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }

    //Adding these functions to test for vendor - Will
    public void AddHealth()
    {
    	HealthPotion potion = (HealthPotion)Instantiate(items[1]);
        AddItem(potion);
    }

    public void AddMagic()
    {
		MagicPotion potionMagic = (MagicPotion)Instantiate(items[2]);
        AddItem(potionMagic);
    }

    public void AddPoison()
    {
    	PoisonPotion potionPoison = (PoisonPotion)Instantiate(items[3]);
        AddItem(potionPoison);
    }
    //I added this to make it easier to add a new bag from another script.
    public void NewBag(){
    	Bag bag = (Bag)Instantiate(items[0]);
            bag.Intialize(16);
            bag.Use();
    }    

    public void AddBag()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Intialize(16);
        AddItem(bag);
    }
}
