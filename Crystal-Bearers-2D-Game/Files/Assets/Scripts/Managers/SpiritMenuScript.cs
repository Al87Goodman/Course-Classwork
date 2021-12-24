using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritMenuScript : MonoBehaviour
{

    // ItemUseManager Singleton
    private static SpiritMenuScript instance;
    public static SpiritMenuScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpiritMenuScript>();
            }

            return instance;
        }
    }

    // For the GUI Menu:
    [Header("For Spirit Menu GUI (manually add)")]
    [SerializeField] private GameObject playerCrystalsObject;
    [SerializeField] private GameObject playerSpiritObject;
    [SerializeField] private GameObject blankSpiritBar; // formally blankCrystalSlot
    [SerializeField] private GameObject spiritMenu;
    [SerializeField] private Text descriptionText;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject cancelButton;

    public CrystalItem currentCrystal;
    public SpiritData currentSpirit;

    // store player's crytals for displaying
    public List<CrystalItem> crystals;
    public List<SpiritData> spirits;


    // Using Data from Other Scripts
    [Header("Data Being Used")]
    [SerializeField] private Item useItem = null;
    [SerializeField] private string namey;


    // Start is called before the first frame update
    void Start()
    {
        //spirits = playerSpiritObject.GetComponent<SpiritDataScript>().MySpirits; 
    }

    private void OnEnable()
    {
        // Calls the Display and Updates the Colors (active or not)
        crystals = playerCrystalsObject.GetComponent<PlayerCrystalScript>().MyCrystals;
        spirits = playerSpiritObject.GetComponent<SpiritDataScript>().MySpirits;
        MakeSpiritBars();
        SetTextAndButton("Select a Spirit", true);
    }

    private void OnDisable()
    {
        ClearSpiritBars();
    }


    public void SpiritItemMenu(Item MyItem)
    {
        // store item to be used on Spirit
        Debug.Log("-- Item to Be Used: " + MyItem.MyTitle + " --");
        useItem = MyItem;

    }


    public void TossInventoryItem(Item MyItem)
    {
        Debug.Log("-- TOSSING ITEM: " + MyItem.MyTitle + " --");
        // If Item is Tossable
        if (MyItem is ITossable)
        {
            (MyItem as ITossable).Toss();
        }
        // Else if Item is a Bag
        else if (MyItem is Bag)
        {
            MyItem.MySlot.Clear();
            InventoryScript.MyInstance.FromSlot = null;
        }
    }

    public string UseInventoryItem(Item MyItem, SpiritData spirit)
    {
        bool usedItem = false;
        string message = "";
        // If Item is Useable
        if (MyItem is IPotion) // & a Potion... Add IPotion interface?
        {
            usedItem = (MyItem as IPotion).UsePotion(spirit);
            if (!usedItem)
            {
                message = "COULD NOT USE POTION";
            }
            else
            {
                message = "SUCCESSFULLY USED POTION";
            }
        }
        else if (MyItem is Bag)
        {
            // Not Implemented Yet
        }
        else
        {
            message = "ITEM COULD NOT BE USED";
        }

        return message;
    }

    // Returns the index from the spirits array passed in.
    private int GetSpiritIndex(string namey)
    {
        int index = -1;
        for (int i = 0; i < spirits.Count; i++)
        {
            if (spirits[i].spiritName == namey)
            {
                index = i;
            }
        }
        return index;
    }

    /* -- Spirit Item Menu Manager (Did Not Want to Create a New Script) -- */

    // Set to Default View
    public void SetTextAndButton(string description, bool isCrystalActive)
    {
        // Empty Description
        descriptionText.text = description;

        // Deactivate all buttons
        useButton.SetActive(false);
        cancelButton.SetActive(false);


        // Recolor the Crystal Slots show if they are active or stored
        for (int i = 0; i < crystals.Count; i++)
        {
            if (crystals[i].isActive)
            {
                transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).GetComponent<Image>().color = new Color(0, 255, 101);
            }
            else if (crystals[i].numberHeld > 0)
            {
                transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255);
            }
        }

    }

    // Setup crystal slots  
    void MakeSpiritBars()
    {
        if (crystals.Count > 0)
        {
            // Go Through Player's Crystal Bag and if Crystal Exist, Create a Slot for it.
            foreach (var crystal in crystals)
            {
                // Remove this to show crystals that were at zero (meaning traces are still in crystal bag)
                if (crystal.numberHeld > 0)
                {
                    int index = GetSpiritIndex(crystal.crystalName);
                    //Debug.Log("Spirit Nameyy: " + spirits[index].spiritName);
                    GameObject tmp = Instantiate(blankSpiritBar, spiritMenu.transform.position, Quaternion.identity, spiritMenu.transform);
                    SpiritBar newSlot = tmp.GetComponent<SpiritBar>();
                    if (newSlot)
                    {
                        newSlot.Setup(crystal, this, spirits[index]);
                    }
                }
            }
        }
    }


    // Load the Crystal Information (Currently can only use Potions)
    public void SetupDescriptionAndButton(string spiritName, bool isCrystalActive, CrystalItem newCrystal)
    {
        if (useItem != null)
        {
            currentCrystal = newCrystal;
            descriptionText.text = string.Format("Use {0} on {1} ?", useItem.MyTitle, spiritName); //"Use " + useItem.MyTitle + " on" + spiritName + "?";
            cancelButton.SetActive(true);
            useButton.SetActive(true);
        }

    }

    void ClearSpiritBars()
    {
        for (int i = 0; i < spiritMenu.transform.childCount; i++)
        {
            Destroy(spiritMenu.transform.GetChild(i).gameObject);
        }
    }

    // Use & Cancel Buttons
    public void UseButtonPressed()
    {
        if (currentCrystal)
        {
            int index = GetSpiritIndex(currentCrystal.crystalName);
            string descriptionMessage = "";
            //spirits[index].HP -= 20; // used in testing purposes...
            //spirits[index].MP -= 20; // used in testing purposes...
            descriptionMessage = UseInventoryItem(useItem, spirits[index]);

            ClearSpiritBars();
            MakeSpiritBars();
            SetTextAndButton(descriptionMessage, true);
            useItem = null;
        }
    }
    public void CancelButtonPressed()
    {
        if (currentCrystal)
        {
            SetTextAndButton("Select a Spirit", false);
        }
    }


}
