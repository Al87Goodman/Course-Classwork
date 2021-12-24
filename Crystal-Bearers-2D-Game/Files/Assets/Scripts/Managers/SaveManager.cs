using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private Item[] items;
    [SerializeField] private CrystalItem[] crystals;

    // contains the ... well ... saved slots
    [SerializeField] private SavedGame[] saveSlots;

    // Dialog Box In the Save Menu 
   [SerializeField] private GameObject dialogue;
   [SerializeField] private Text dialogueText;

    private SavedGame current;
    private string action; // load, save, delete
    public GameInfo gameInfo;

    //private MovePlayer player;

    // Initialization
    void Awake()
    {
        //Debug.Log(Application.persistentDataPath);
        foreach (SavedGame saved in saveSlots)
        {
            // show saved 
            ShowSavedFiles(saved);
        }
    }

    private void Start()
    {
        // If a load slot was selected
        if (PlayerPrefs.HasKey("Load"))
        {
            Load(saveSlots[PlayerPrefs.GetInt("Load")]);
            PlayerPrefs.DeleteKey("Load"); // will stop duplicates 
        }
        // Else setup default/new game 
        else if (SceneManager.GetActiveScene().name != "StartMenu")
        {
            // Remove this and uncomment out try-catch if Player Object is removed from MainMenu Scene
            //MovePlayer.MyInstance.DefaultValues();

            try
            {
                // Set Default Playe Values 
                MovePlayer.MyInstance.DefaultValues();
            }
            // For Main Menu Loading New Game.
            catch (Exception)
            {

                
            }

        }
    }
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) { Save();}
        if (Input.GetKeyDown(KeyCode.R)) { Load(saveSlots[0]);}
    }
    */

    /* -- SHOW SECTION -- */

    // Show the Saved Data to the User (Populate the Save Panel UI)
    private void ShowSavedFiles(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            savedGame.ShowInfo(data);
        }
        else
        {
            savedGame.HideInfo();
        }

    }

    // Dialog for asking if you want to save, load, delete a game
    public void ShowDialogue(GameObject clickOption)
    {
        action = clickOption.name;
        switch (action)
        {
            case "Load":
                dialogueText.text = "Load Game?";
                break;
            case "Save":
                dialogueText.text = "Save Game?";
                break;
            case "Delete":
                dialogueText.text = "Delete this File?";
                break;
            default:
                break;
        }

        current = clickOption.GetComponentInParent<SavedGame>();
        dialogue.SetActive(true);
    }

    // Close the Dialogue Box
    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }

    // Executes the Clicked on Action (Load, Save, or Delete)
    public void ExecuteAction()
    {
        switch (action)
        {
            case "Load":
                //Load(current);
                LoadScene(current);
                break;
            case "Save":
                Save(current);
                break;
            case "Delete":
                Delete(current);
                break;
            default:
                break;
        }

        CloseDialogue();
    }

    private void LoadScene(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            gameInfo.RespawnLocation = data.MyScene;
            PlayerPrefs.SetInt("Load", savedGame.MyIndex); // 0,1,2 --> set in the Inspector
            SceneManager.LoadScene(data.MyScene);
            Time.timeScale = 1f; // if not added, then player cannot move...
        }
        else
        {
            savedGame.HideInfo();
        }
    }

   /* -- DELETE SECTION -- */

    //Deletes the Game File
    private void Delete(SavedGame savedGame)
    {
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
        savedGame.HideInfo();
    }

    /* -- SAVE SECTION -- */

    // Save the Game Information (Main Save Section)
    public void Save(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            // Save Data to Local Drive
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);
            // Instantiate Data
            SaveData data = new SaveData();

            // Get the name of the current scene
            data.MyScene = SceneManager.GetActiveScene().name;

            /* -- SECTIONS -- */
            SaveBags(data);
            SaveIntentory(data);
            SavePlayer(data);
            SavePlayerCrystals(data);
            SavePlayerSpirits(data);

            // Serialize
            bf.Serialize(file, data);
            // Close FileStream
            file.Close();
            ShowSavedFiles(savedGame);

        }
        catch (System.Exception)
        {
            Delete(savedGame);
            PlayerPrefs.DeleteKey("Load");
            //throw;
        }
    }

    private void SavePlayer(SaveData data)
    {
        // Access Move Player Info
        data.MyPlayerData = new PlayerData(MovePlayer.MyInstance.transform.position);
    }


    private void SaveBags(SaveData data)
    {
        // Starting at 1 because first bag is a standard default bag
        for (int i = 0; i < InventoryScript.MyInstance.MyBags.Count; i++)
        {
            data.MyInventoryData.MyBags.Add(new BagData(InventoryScript.MyInstance.MyBags[i].MySlotCount, InventoryScript.MyInstance.MyBags[i].MyBagButton.MyBagIndex));
        }
    }

    private void SaveIntentory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.MyInstance.GetAllItems();
        data.MyInventoryData.MyCoins = InventoryScript.MyInstance.GetCoins();

        foreach (SlotScript slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyTitle, slot.MyItems.Count, slot.MyIndex, slot.MyBag.MyBagIndex));
        }
    }

    // Save Crystals
    private void SavePlayerCrystals(SaveData data)
    {
        List<CrystalItem> items = PlayerCrystalScript.MyInstance.MyCrystals; 
        foreach (CrystalItem item in items)
        {
            data.MyCrystalData.MyCrystals.Add(new CrystalData(item.crystalID, item.crystalName, item.numberHeld, item.isActive));
        }
    }

    // Save Spirits
    private void SavePlayerSpirits(SaveData data)
    {
        List<SpiritData> spirits = SpiritDataScript.MyInstance.MySpirits;
        data.MySpiritData.MySpirits = spirits;
    }


    /* -- LOAD SECTION -- */

    private void Load(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            // Save Data to Local Drive
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            // Instantiate Data
            SaveData data = (SaveData)bf.Deserialize(file);

            // Close FileStream
            file.Close();
            LoadBags(data); // need to be first
            LoadInventory(data);
            LoadPlayer(data);
            LoadPlayerCrystals(data);
            LoadPlayerSpirits(data);
        }
        catch (System.Exception)
        {
            // This is for Handling Corrupted Data and Errors
            //Delete(savedGame);
            //PlayerPrefs.DeleteKey("Load");
            throw;
        }
    }

    // Set Player Information 
    private void LoadPlayer(SaveData data)
    {
        //MovePlayer.MyInstance.DefaultValues();
        MovePlayer.MyInstance.transform.position = new Vector3(data.MyPlayerData.MyX, data.MyPlayerData.MyY, data.MyPlayerData.MyZ);
    }

    // Load Bags
    private void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.MyInventoryData.MyBags)
        {   //Important to have bag at element 0 in SaveManager in Inspector
            Bag newBag = (Bag)Instantiate(items[0]);

            // Create a new bag
            newBag.Intialize(bagData.MySlotCount);

            // Add to inventory
            InventoryScript.MyInstance.AddBag(newBag, bagData.MyBagIndex);
        }
    }

    // Load Inventory
    private void LoadInventory(SaveData data)
    {
        Debug.Log("Coins Loaded: " + data.MyInventoryData.MyCoins);
        InventoryScript.MyInstance.AddCoins(data.MyInventoryData.MyCoins);

        foreach (ItemData itemData in data.MyInventoryData.MyItems)
        {
            // Locate the item saved 
            Item item = Array.Find(items, x => x.MyTitle == itemData.MyTitle);

            // Place item correctly in the Inventory
            for (int i = 0; i < itemData.MyStackCount; i++)
            {
                InventoryScript.MyInstance.PlaceInSpecific(item, itemData.MySlotIndex, itemData.MyBagIndex);
            }
        }
    }

    // Load Player Crystals
    private void LoadPlayerCrystals(SaveData data)
    {
        CrystalItem newCrystal = null;
        foreach (CrystalData crystalData in data.MyCrystalData.MyCrystals)
        {
            // Locate the crystal saved 
            for (int i = 0; i < crystals.Length; i++)
            {
                if (crystalData.MyID == crystals[i].crystalID)
                {
                    newCrystal = Instantiate(crystals[i]);
                }
            }
            PlayerCrystalScript.MyInstance.LoadCrystals(newCrystal, crystalData.MyName, crystalData.MyNumberHeld, crystalData.MyIsActive);
        }

    }

    private void LoadPlayerSpirits(SaveData data)
    {
        SpiritDataScript.MyInstance.MySpirits = data.MySpiritData.MySpirits;
    }

    // Use in the Main Menu Screen to Go Back to the Main Menu
    public string mainMenu;
    public void QuitToMain()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f; // Animations run off of this
    }
}
