using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritDataScript : MonoBehaviour
{

    // Singleton 
    private static SpiritDataScript instance;
    public static SpiritDataScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpiritDataScript>();
            }

            return instance;
        }
    }

    // Player Spirits (Total should match exactly with number of Player Crystals)
    [SerializeField] private List<SpiritData> playerSpirits;

    public List<SpiritData> MySpirits { get => playerSpirits; set => playerSpirits = value; }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Add a New Spirit (Usually called from PlayerCrystalScript?)
    public void AddSpirit(string spiritName, string objectName, bool active = false)
    {
        SpiritData data = new SpiritData();
        data.objectName = objectName;
        data.spiritName = spiritName;
        //data.isActive = active;
        // Remove Hard Code and Loop through looking for objectName and Index. OR call playerpartymanager and return 
        var spiritObj = PlayerPartyManager.MyInstance.NewSpirit(data, objectName, spiritName);
        data.isActive = active;
        //playerSpirits.Add(spiritObj);
        playerSpirits.Add(data);
    }


    // Remove a Spirit (Usually Called from PlayerCrystalsScript)
    public void RemoveSpirit(string spiritName)
    {
        foreach (SpiritData spirit in playerSpirits)
        {
            if (spirit.spiritName == spiritName)
            {
                playerSpirits.Remove(spirit);
                return;
            }
        }
    }

    // Load All Spirits


    // Change Active/Store (Call in CrystalManager ...don't like it)
    public void FlipActive(string spiritName)
    {
        foreach (SpiritData spirit in playerSpirits)
        {
            if (spirit.spiritName == spiritName)
            {
                spirit.isActive = !spirit.isActive;
                return;
            }
        }
    }

    public bool AbleToBattle()
    {
        bool ableToBattle = false;

        foreach (SpiritData spirit in playerSpirits)
        {
            if (spirit.isActive && spirit.HP > 0)
            {
                ableToBattle = true;
            }
        }

        return ableToBattle;
    }

}
