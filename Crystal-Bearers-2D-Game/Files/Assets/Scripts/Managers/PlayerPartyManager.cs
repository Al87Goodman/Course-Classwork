using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartyManager : MonoBehaviour
{
    // Singleton 
    private static PlayerPartyManager instance;
    public static PlayerPartyManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerPartyManager>();
            }

            return instance;
        }
    }

    // Spirits - Manually Add in the Inspector
    [SerializeField] private GameObject[] spirits = null;

    // Dynamically Add Players to the Party 
    public List<GameObject> playerParty = null;

    // Holds list of Players Spirits
    private List<SpiritData> playerSpirits = null;

    // Retrieve/Set List of the Current Party Members
    public List<GameObject> MyPlayerParty { get => playerParty; set => playerParty = value; }



    // summon Activated Spirits  -> Used right before Battles
    public List<GameObject> SummonSpirits()
    {
        // Retrieve List of Player's Spirits
        playerSpirits = SpiritDataScript.MyInstance.MySpirits;
        Debug.Log("COUNT: " + playerSpirits.Count);
        foreach (SpiritData spirit in playerSpirits)
        {
            if (spirit.isActive && spirit.HP > 0)
            {
                // locate spirit object by type/ID, Instantiate, and Set Attributes.
                GameObject newSpirit = null; 
                foreach (var spiritObj in spirits)
                {
                    if (spirit.objectName == spiritObj.name)
                    {
                        newSpirit = Instantiate(spirits[GetSpiritIndex(spiritObj.name)]);
                        //newSpirit = Instantiate(spirits[spiritObj.GetComponent<UnitStats>().ID-1]);
                        newSpirit = SpiritDataToObject(spirit, newSpirit);
                        Debug.Log("Added Spirit: " + spirit.objectName + "Spirit ID: " + spirit.objectID);
                        AddPartyMember(newSpirit);
                    }
                    
                }
            }
        }

        return playerParty;

    }

    // Save Data Before Dismissing Spirits --> Used directly after Battles before Scene Changes
    // May need to take in a List of GameObjects (Containing the Spirits from the Battle)
    public void DismissSpirits(bool win = true)
    {
        // Save Battle Data: Spirit Game Object to the Spirit Data
        foreach (GameObject member in playerParty)
        {
            // If Loss, then heal all party members.
            if (!win)
            {
                member.GetComponent<UnitStats>().revive();
            }
            //heal all party members
            //member.GetComponent<UnitStats>().revive();

            string objName = member.GetComponent<UnitStats>().spiritName;
            for (int i = 0; i < playerSpirits.Count; i++)
            {
                if (objName == playerSpirits[i].spiritName)
                {
                    playerSpirits[i] = SpiritObjectToData(playerSpirits[i], member);
                }
            }
        }
        // Update the Persistent Data Spirits
        UpdateSpirits();

        // Destroy Party Members
        RemovePartyMembers();
    }

    // Load spirit data to Spirit Game Objects
    public GameObject SpiritDataToObject(SpiritData spirit, GameObject spiritObj)
    {
        spiritObj.GetComponent<UnitStats>().spiritName = spirit.spiritName;
        spiritObj.GetComponent<UnitStats>().ID = spirit.objectID;

        spiritObj.GetComponent<UnitStats>().HP = spirit.HP;
        spiritObj.GetComponent<UnitStats>().maxHP = spirit.maxHP;
        spiritObj.GetComponent<UnitStats>().MP = spirit.MP;
        spiritObj.GetComponent<UnitStats>().maxMP = spirit.maxMP;
        spiritObj.GetComponent<UnitStats>().attack = spirit.attack;
        spiritObj.GetComponent<UnitStats>().magic = spirit.magic;
        spiritObj.GetComponent<UnitStats>().defense = spirit.defense;
        spiritObj.GetComponent<UnitStats>().magDef = spirit.magDef;
        spiritObj.GetComponent<UnitStats>().speed = spirit.speed;

        spiritObj.GetComponent<UnitStats>().MyLevel = spirit.level;
        spiritObj.GetComponent<UnitStats>().MyTotalExp = spirit.totalExp;
        spiritObj.GetComponent<UnitStats>().MyNextLevel = spirit.nextLevel;

        return spiritObj;
    }

    // Load Spirit Object into Spirit Data
    public SpiritData SpiritObjectToData(SpiritData spirit, GameObject spiritObj)
    {
        spirit.objectID = spiritObj.GetComponent<UnitStats>().ID;
        spirit.isActive = true;

        spirit.HP = spiritObj.GetComponent<UnitStats>().HP;
        spirit.maxHP = spiritObj.GetComponent<UnitStats>().maxHP;
        spirit.MP = spiritObj.GetComponent<UnitStats>().MP;
        spirit.maxMP = spiritObj.GetComponent<UnitStats>().maxMP;
        spirit.attack = spiritObj.GetComponent<UnitStats>().attack;
        spirit.magic = spiritObj.GetComponent<UnitStats>().magic;
        spirit.defense = spiritObj.GetComponent<UnitStats>().defense;
        spirit.magDef = spiritObj.GetComponent<UnitStats>().magDef;
        spirit.speed = spiritObj.GetComponent<UnitStats>().speed;

        spirit.level = spiritObj.GetComponent<UnitStats>().MyLevel;
        spirit.totalExp = spiritObj.GetComponent<UnitStats>().MyTotalExp; 
        spirit.nextLevel = spiritObj.GetComponent<UnitStats>().MyNextLevel;

        return spirit;
    }

    // Load Spirit Object into Spirit Data (Mainly Used in SpiritDataScript)
    public SpiritData NewSpirit(SpiritData spirit, string objName, string spirName)
    {
        // Locate the Game Object Associated with the Crystal and Set the Default Data.
        foreach (GameObject objectSpirit in spirits)
        {
            if (objectSpirit.name == objName)
            {
               GameObject spiritObj = Instantiate(spirits[GetSpiritIndex(objName)]);
                spirit.spiritName = spirName;
                spirit.objectID = spiritObj.GetComponent<UnitStats>().ID;
                spirit.isActive = false;

                //Debug.Log("New Spirit Created: " + spirit.spiritName + " ; ID: " + spirit.objectID);

                spirit.HP = spiritObj.GetComponent<UnitStats>().HP;
                spirit.maxHP = spiritObj.GetComponent<UnitStats>().maxHP;
                spirit.MP = spiritObj.GetComponent<UnitStats>().MP;
                spirit.maxMP = spiritObj.GetComponent<UnitStats>().maxMP;
                spirit.attack = spiritObj.GetComponent<UnitStats>().attack;
                spirit.magic = spiritObj.GetComponent<UnitStats>().magic;
                spirit.defense = spiritObj.GetComponent<UnitStats>().defense;
                spirit.magDef = spiritObj.GetComponent<UnitStats>().magDef;
                spirit.speed = spiritObj.GetComponent<UnitStats>().speed;

                spirit.level = spiritObj.GetComponent<UnitStats>().MyLevel;
                spirit.totalExp = spiritObj.GetComponent<UnitStats>().MyTotalExp;
                spirit.nextLevel = spiritObj.GetComponent<UnitStats>().MyNextLevel;
                Destroy(spiritObj);
                return spirit;
            }
        }
        return spirit;
    }

    // Add Instantiate Spirit Object to the Player Party List
    public void AddPartyMember(GameObject newMember)
    {
        playerParty.Add(newMember);
    }

    // Update the Spirit Data Back in the spirit data script.
    public void UpdateSpirits()
    {
        // New Data.
        SpiritDataScript.MyInstance.MySpirits = playerSpirits;
    }

    public void RemovePartyMembers()
    {
        foreach (GameObject member in playerParty)
        {
            Destroy(member);
        }
    }

    public int GetSpiritIndex(string objectName)
    {
        int index = -1;
        for (int i = 0; i < spirits.Length; i++)
        {

            if (spirits[i].GetComponent<UnitStats>().spiritName == objectName)
            {
                index = i;
            }

        }
        return index;
    }




    /* --- TESTING PURPOSES ONLY  --- */
    // Display Party Member Stats
    public void DisplayPartyMemberStats(GameObject spirit)
    {
        Debug.Log("Member ID: " + spirit.GetComponent<UnitStats>().ID);
        Debug.Log("Member HP: " + spirit.GetComponent<UnitStats>().HP);
    }

    // Display Party Member Stats
    public void LowerPartyMemberHP(GameObject spirit)
    {

        spirit.GetComponent<UnitStats>().HP -= 1;
    }

}
