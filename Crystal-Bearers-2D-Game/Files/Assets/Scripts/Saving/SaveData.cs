using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Hold Crystals and Other Scriptable Objects.
    //public List<ScriptableObject> objects = new List<ScriptableObject>();


    public PlayerData MyPlayerData { get; set; }
    public InventoryData MyInventoryData { get; set; }
    public PlayerCrystalData MyCrystalData { get; set; }
    public PlayerSpiritData MySpiritData { get; set; }
    public DateTime MyDateTime { get; set; }
    public string MyScene { get; set; }

    // Constructor
    public SaveData()
    {
        // Instantiate Inventory Data
        MyInventoryData = new InventoryData();
        MyCrystalData = new PlayerCrystalData();
        MySpiritData = new PlayerSpiritData();
        MyDateTime = DateTime.Now;
    }

}


// Player Data
[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }
    public float MyX { get; set; }
    public float MyY { get; set; }
    public float MyZ { get; set; }

    public PlayerData(Vector3 position)
    {
        //this.MyLevel = level;
        this.MyX = position.x;
        this.MyY = position.y;
        this.MyZ = position.z;
    }

}

// Item Data
[Serializable]
public class ItemData
{
    public string MyTitle { get; set; }

    public int MyStackCount { get; set; }

    public int MySlotIndex { get; set; }

    public int MyBagIndex { get; set; }

    public ItemData(string title, int stackCount=0, int slotIndex=0, int bagIndex=0)
    {

        MyTitle = title;
        MyStackCount = stackCount;
        MySlotIndex = slotIndex;
        MyBagIndex = bagIndex;

    }
}

// Inventory Data
[Serializable]
public class InventoryData
{
    public List<BagData> MyBags { get; set; }

    public List<ItemData>MyItems { get; set; } // contain all items in the inventory

    public int MyCoins { get; set; }

    public InventoryData()
    {
        MyBags = new List<BagData>();
        MyItems = new List<ItemData>();
        MyCoins = 0;
    }
}

// Bag Data
[Serializable]
public class BagData
{
    public int MySlotCount { get; set; }
    public int MyBagIndex { get; set; }

    public BagData(int slotCount, int bagIndex)
    {
        MySlotCount = slotCount;
        MyBagIndex = bagIndex;
    }
}

// Crystal Data
[Serializable]
public class CrystalData
{
    public int MyID { get; set; }
    public string MyName { get; set; }

    //public string MyDescription{ get; set; }

    public int MyNumberHeld { get; set; }

    public bool MyIsActive { get; set; }

    //public bool MyIsUnique { get; set; }

    public CrystalData(int id,  string title, int stackCount = 0, bool active = false)
    {
        MyID = id;
        MyName = title;
        MyNumberHeld = stackCount;
        MyIsActive = active;
    }
}

// Player Crystal Data
[Serializable]
public class PlayerCrystalData
{
    public List<CrystalData> MyCrystals { get; set; } // contains the players crytsals

    public PlayerCrystalData()
    {
        MyCrystals= new List<CrystalData>();
    }
}

// Player Spirit Data
[Serializable]
public class PlayerSpiritData
{
    public List<SpiritData> MySpirits{ get; set; } // contains the players crytsals

    public PlayerSpiritData()
    {
        MySpirits = new List<SpiritData>();
    }
}


