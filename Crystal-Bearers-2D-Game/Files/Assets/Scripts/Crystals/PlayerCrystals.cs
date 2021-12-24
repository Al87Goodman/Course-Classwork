using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WARNING - Now Defunct: No longer used due to gameplay changes being permanent 

// Add into Scriptable Objects Folder. Will be able to edit the player's crystals without creating Singletons!!
[System.Serializable]
[CreateAssetMenu(fileName = "NewCrystalBag", menuName = "Inventory/Player Crystal Bag")]
public class PlayerCrystals : ScriptableObject
{
    public List<CrystalItem> myCrystals = new List<CrystalItem>();
}
