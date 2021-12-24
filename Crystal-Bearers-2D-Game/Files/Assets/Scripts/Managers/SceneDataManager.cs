using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataManager : MonoBehaviour
{
    // Static Reference
    private static SceneDataManager instance;
    public static SceneDataManager MyInstance { get { return instance; } }

    // Data to Persist
    [SerializeField] private Item[] items;
    [SerializeField] private CrystalItem[] crystals;
    SaveData sceneData = new SaveData();


    private string currentSceneName
    {
        get
        {
            return SceneManager.GetActiveScene().name;
        }
    }

  
    private void Awake()
    {
        //Debug.Log("SceneDataManager: Awake()");
        if (instance == null && currentSceneName != "StartMenu")
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); 
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    // Get/Set sceneData
    public SaveData MySceneData { get => sceneData; set => sceneData = value; }

    /* -- SCENE TRANSITION SECTION -- */

    // Scene Transition (Save Data, Load Scene, Load Data)
    public void SceneTransition(string scene)
    {
        // Save the Scene Data
        SaveSceneData();

        // Load the Scene & Wait Until Scene is Loaded before Loading Data
        SceneManager.LoadScene(scene);
        if (currentSceneName != scene)
        {
            //Debug.Log("Start Scene Load Coroutine");
            StartCoroutine("waitForSceneLoad", scene);
        }
        else
        {
            LoadSceneData();
        }

    }

   private IEnumerator waitForSceneLoad(string scene)
    {
        while (currentSceneName != scene)
        {
            yield return null;
        }
        if (currentSceneName == scene)
        {
            // Load Scene Data
            LoadSceneData();
        }
    }

    // Battle Scene Transition (Save Data, Load Scene, Load Data)
    public bool BattleSceneTransition()
    {
        LoadSceneData();

        return true;
    }


    /* -- SAVE SCENE DATA SECTION -- */

    // Save Scene Data
    private void SaveSceneData()
    {
        try
        {
            // Save Bags, Inventory, and then store the data. 
            //Debug.Log("SceneDataManager: SaveSceneData()");
            SaveData data = new SaveData(); 
            SaveBags(data);
            SaveIntentory(data);
            try
            {
                SavePlayer(data);
            }
            catch (Exception)
            {

                Debug.Log("Scene Data Failed to Save Player Information");
            }

            SavePlayerCrystals(data);
            SavePlayerSpirits(data);
            MySceneData = data; // overwrite previously held data to avoid duplicates

        }
        catch (System.Exception)
        {
            // delete the
            //PlayerPrefs.DeleteKey("Load");
            Debug.Log("Scene Data Failed to Save");
            //throw;
        }
    }

    // Save Bags
    private void SaveBags(SaveData data)
    {
        // (formally started at 1 becauase used default bag)
            for (int i = 0; i < InventoryScript.MyInstance.MyBags.Count; i++)
            {
                data.MyInventoryData.MyBags.Add(new BagData(InventoryScript.MyInstance.MyBags[i].MySlotCount, InventoryScript.MyInstance.MyBags[i].MyBagButton.MyBagIndex));
            }

    }

    // Save Inventory 
    private void SaveIntentory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.MyInstance.GetAllItems();
        data.MyInventoryData.MyCoins = InventoryScript.MyInstance.GetCoins();
        foreach (SlotScript slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyTitle, slot.MyItems.Count, slot.MyIndex, slot.MyBag.MyBagIndex));
        }
    }

    private void SavePlayer(SaveData data)
    {
        // Access Move Player Info
        data.MyPlayerData = new PlayerData(MovePlayer.MyInstance.transform.position);
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

    /* -- LOAD SCENE DATA SECTION -- */

    // Load Scene Data
    private void LoadSceneData()
    {
        try
        {
            if (currentSceneName == "StartMenu" && instance != null)
            {
                Debug.Log("SceneDataManager LoadScenedata(): Remove Game Object");
                Destroy(gameObject);
            }
            // Get Saved Scene Data and load it in!
            //Debug.Log("SceneDataManager: LoadSceneData()");
            LoadBags(MySceneData); // need to be first
            LoadInventory(MySceneData);
            //LoadPlayer(MySceneData);
            LoadPlayerCrystals(MySceneData);
            LoadPlayerSpirits(MySceneData);
        }
        catch (System.Exception)
        {
            // This is for Handling Corrupted Data and Errors
            //Delete(savedGame);
            //PlayerPrefs.DeleteKey("Load");
            throw;
        }
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
        InventoryScript.MyInstance.OpenClose();
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

    // Set Player Information 
    private void LoadPlayer(SaveData data)
    {
        MovePlayer.MyInstance.DefaultValues();
        //MovePlayer.MyInstance.transform.position = new Vector3(data.MyPlayerData.MyX, data.MyPlayerData.MyY, data.MyPlayerData.MyZ);
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
            //Debug.Log(crystalData.MyName);
            PlayerCrystalScript.MyInstance.LoadCrystals(newCrystal, crystalData.MyName, crystalData.MyNumberHeld, crystalData.MyIsActive);
        }
    }

    private void LoadPlayerSpirits(SaveData data)
    {
        SpiritDataScript.MyInstance.MySpirits = data.MySpiritData.MySpirits;
    }

    // Delete the Scene Data
    public void DeleteSceneData()
    {
        //Debug.Log("SceneDataManager: Remove Game Object");
        Destroy(gameObject);        
    }




}
