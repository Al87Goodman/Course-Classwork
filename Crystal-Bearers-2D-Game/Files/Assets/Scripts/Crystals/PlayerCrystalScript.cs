using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrystalScript : MonoBehaviour
{
    // Singleton 
    private static PlayerCrystalScript instance;
    public static PlayerCrystalScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerCrystalScript>();
            }

            return instance;
        }
    }

    // Crystal Assets
    [SerializeField] private CrystalItem[] crystals = null;

    // Player Crystals
    //[SerializeField] private List<CrystalItem> playerCrystals;
    public List<CrystalItem> playerCrystals;

    public List<CrystalItem> MyCrystals { get => playerCrystals; set => playerCrystals = value; }

    // Update is called once per frame
    void Update()
    {

        //NOTE: DEBUG ONLY - Add a New Crystal
        if (Input.GetKeyDown(KeyCode.X))
        {
            CrystalItem crystal = (CrystalItem)Instantiate(crystals[5]);
            AddCrystal(crystal);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CrystalItem crystal = (CrystalItem)Instantiate(crystals[6]);
            AddCrystal(crystal);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CrystalItem crystal = (CrystalItem)Instantiate(crystals[7]);
            AddCrystal(crystal);
        }
        */
    }

    // Add a Crystal to the Player's Crystal Bag/List
    public void AddCrystal(CrystalItem crystal)
    {
        int sameName = 0;
        string originalName = crystal.crystalName;
        // Find if Crystal Name Exists - make unique name and add to List
        int i = 0;
        while(i < playerCrystals.Count)
        {
                if (crystal.crystalName == playerCrystals[i].crystalName)
                {
                    sameName++;
                    crystal.crystalName = originalName + sameName.ToString();
                    i = 0; // reset loop
                }
                i++;
        }

        // Add a maximum count here? if playerCrystals.Count < 4 set as Active?
        crystal.numberHeld = 1;
        crystal.isActive = false;
        playerCrystals.Add(crystal);
        SpiritDataScript.MyInstance.AddSpirit(crystal.crystalName, crystal.spiritObject); // Need to Add Spirit Data.
    }

    public void AddCrystalByName(string crystalName)
    {
        int index = CrystalIndex(crystalName);
        if (index >= 0)
        {
            CrystalItem crystalNew = (CrystalItem)Instantiate(crystals[CrystalIndex(crystalName)]);
            AddCrystal(crystalNew);
        }
        else
        {
            Debug.Log("COULD NOT FIND CRYSTAL BY NAME");
        }

    }

    public void AddStarterCrystal(string crystalName)
    {
        int index = CrystalIndex(crystalName);
        if (index >= 0)
        {
            CrystalItem crystalNew = (CrystalItem)Instantiate(crystals[CrystalIndex(crystalName)]);
            //crystalNew.crystalName =crystalNew.crystalName;
            crystalNew.numberHeld = 1;
            crystalNew.isActive = true;
            crystalNew.unique = true;
            playerCrystals.Add(crystalNew);
            SpiritDataScript.MyInstance.AddSpirit(crystalNew.crystalName, crystalNew.spiritObject, crystalNew.isActive); // Need to Add Spirit Data.
        }
        else
        {
            Debug.Log("COULD NOT FIND CRYSTAL BY NAME");
        }

    }

    // Load Crystals from storage (temp or perm)
    public void LoadCrystals(CrystalItem crystal, string name, int count, bool active)
    {
        crystal.crystalName = name;
        crystal.numberHeld = count;
        crystal.isActive = active;
        playerCrystals.Add(crystal);
    }

    // Remove Crystal
    public void RemoveCrystal(CrystalItem crystal)
    {
        SpiritDataScript.MyInstance.RemoveSpirit(crystal.crystalName); // Need to Remove Spirit Data.
        playerCrystals.Remove(crystal);
    }

    // Re-Organize Crystal Bag List and have the Active Crystals First? (That way always displays first in bag)

    // Get Number of Active Crystals


    // TEST TEST SCRIPT
    public void CrystalTest()
    {
        Debug.Log("TESTING PLAYER CRYSTAL SCRIPT");
        //Debug.Log(playerCrystals[0].crystalName);
    }

    // Crystal Index
    private int CrystalIndex(string newCrystalName)
    {
        int index = -1;
        for (int i = 0; i < crystals.Length; i++)
        {
            if (crystals[i].spiritObject == newCrystalName)
            {
                index = i;
            }
        }

        return index;
    }

}
