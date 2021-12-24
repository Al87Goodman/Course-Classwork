using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseManager : MonoBehaviour
{
    // ItemUseManager Singleton
    private static ItemUseManager instance;
    public static ItemUseManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemUseManager>();
            }

            return instance;
        }
    }

    // Display the DIfferent Associate Panels/Menus
    [Header("Game Objects For UI Spirit-Item Usage")]
    public GameObject itemOptionMenu;
    public GameObject spiritSelectionMenu;
    // add game object menu for entering during Battle 

    private Item MyItem;
    
    // Hide the Item Use Menues on Start
    void Start()
    {
        itemOptionMenu.SetActive(false);
        spiritSelectionMenu.SetActive(false);
    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Pause the Game?

    }

    // Pause Function Here if Desired


    // Open-Close Item Option Menu 
    public void CloseItemOptionMenu()
    {
        itemOptionMenu.SetActive(false);
        spiritSelectionMenu.SetActive(false);
    }
    public void OpenItemOptionMenu(Vector3 position, Item item)
    {
        // Only Display for Useable Items at the moment.... Need to create another panel for Tossing UnUseable Items
        if (item is IUseable && item is IPotion)// & !itemOptionMenu.activeSelf)
        {
            itemOptionMenu.SetActive(true);
            itemOptionMenu.transform.position = position;
            Debug.Log("Item Name:" + item.MyTitle);
            spiritSelectionMenu.SetActive(false);
            MyItem = item;
        }

        else
        {
            itemOptionMenu.SetActive(true);
            itemOptionMenu.transform.position = position;
            spiritSelectionMenu.SetActive(false);
            MyItem = item;
        }

    }
    
    // Open-Close Spirit Selection Menu
    // Toss Button
    public void CloseSpiritSelectionMenu()
    {
        spiritSelectionMenu.SetActive(false);
        itemOptionMenu.SetActive(false);
    }

    // Use Button
    public void UseItem()
    {
        Debug.Log("Item to Use: " + MyItem.MyTitle);
        if (MyItem is IPotion) 
        {
            itemOptionMenu.SetActive(false);
            spiritSelectionMenu.SetActive(true);
            SpiritMenuScript.MyInstance.SpiritItemMenu(MyItem);
        }
        // Else if (MyItem is Bag)
        else
        {
            Debug.Log("ITEM IS NOT USUABLE ON A SPIRIT");
            // Display Message That Item Is Not Allowed to Be Used
        }

    }
    // Toss Button
    public void TossItem()
    {
        //Debug.Log("-- TOSSING ITEM: " + MyItem.MyTitle + " --");
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

        itemOptionMenu.SetActive(false);
        spiritSelectionMenu.SetActive(false);

    }

}
