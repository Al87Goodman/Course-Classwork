using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrystalManager : MonoBehaviour
{
    // Player's Current Crystals
    [Header("Crystal Information")]
    [SerializeField] private GameObject playerCrystalsObject;
    [SerializeField] private GameObject playerSpiritObject;
    [SerializeField] private GameObject blankCrystalSlot;
    [SerializeField] private GameObject crystalPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    public CrystalItem currentCrystal;
    public SpiritData currentSpirit;

    // Adds/Removes Crystals from being battle available or not
    [SerializeField] private GameObject activateButton;
    [SerializeField] private GameObject storeButton;
    [SerializeField] private GameObject discardButton;
    //[SerializeField] private GameObject itemButton;

    // store player's crytals for displaying
    public List<CrystalItem> crystals;
    public List<SpiritData> spirits;

    // Set to Default View
    public void SetTextAndButton(string description, bool isCrystalActive)
    {
        // Empty Description
        descriptionText.text = description;

        // Deactivate all buttons
        storeButton.SetActive(false);
        activateButton.SetActive(false);
        discardButton.SetActive(false);
        //itemButton.SetActive(false);

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
    void MakeCrystalSlots()
    {
        if(crystals.Count > 0)
        {
            // Go Through Player's Crystal Bag and if Crystal Exist, Create a Slot for it.
            foreach (var crystal in crystals)
            {
                // Remove this to show crystals that were at zero (meaning traces are still in crystal bag)
                if (crystal.numberHeld > 0)
                {
                    int index = GetSpiritIndex(crystal.crystalName);
                    Debug.Log("Spirit Name: " + spirits[index].spiritName);
                    GameObject tmp = Instantiate(blankCrystalSlot, crystalPanel.transform.position, Quaternion.identity, crystalPanel.transform);
                    CrystalSlot newSlot = tmp.GetComponent<CrystalSlot>();
                    if (newSlot)
                    {
                        newSlot.Setup(crystal, this, spirits[index]);
                    }
                }
            }
        }
    }

    // Locate the index in the crystals array where the spirt name matches the cyrystal name
    private int GetSpiritIndex(string crystalName)
    {
        int index = 0;

        for (int i = 0; i < spirits.Count; i++)
        {
            if (spirits[i].spiritName == crystalName)
            {
                return i;
            }
        }

        return index;
    }

    private int ActiveCrystalCount()
    {
        int count = 0;
        foreach (CrystalItem crystal in crystals)
        {
            if (crystal.isActive)
            {
                count++;
            }
        }

        return count;
    }



    private void Start()
    {
        //crystals = playerCrystalsObject.GetComponent<PlayerCrystalScript>().MyCrystals;
        // NOTE: DEBUG
        //MakeCrystalSlots();
        //SetTextAndButton("", true);
    }

    private void OnEnable()
    {
        // Calls the Display and Updates the Colors (active or not)
        crystals = playerCrystalsObject.GetComponent<PlayerCrystalScript>().MyCrystals;
        spirits = playerSpiritObject.GetComponent<SpiritDataScript>().MySpirits;
        MakeCrystalSlots();
        SetTextAndButton("", true);
    }

    private void OnDisable()
    {
        // Clear out the Crystal Bag Window
        ClearCrystalSlots();
    }

    private void Update()
    {
        
    }

    // Load the Crystal Information
    public void SetupDescriptionAndButton(string newDescriptionString, bool isCrystalActive, CrystalItem newCrystal)
    {
        currentCrystal = newCrystal;
        descriptionText.text = newDescriptionString;
        if (isCrystalActive)
        {
            storeButton.SetActive(true);
            activateButton.SetActive(false);
            discardButton.SetActive(false);

        }
        else
        {
            storeButton.SetActive(false);
            activateButton.SetActive(true);
            if (currentCrystal.unique)
            {
                discardButton.SetActive(false);
            }
            else
            {
                discardButton.SetActive(true);
            }
        }
    }

    void ClearCrystalSlots()
    {
        for (int i = 0; i < crystalPanel.transform.childCount; i++)
        {
            Destroy(crystalPanel.transform.GetChild(i).gameObject);
        }
    }


    // Activate, Store, Discard Buttons when Pressed
    public void ActivateButtonPressed()
    {
        if (currentCrystal)
        {
            SpiritDataScript.MyInstance.FlipActive(currentCrystal.crystalName);
            currentCrystal.ActivatePressed();
            SetTextAndButton("", true);
        }
    }
    public void StoreButtonPressed()
    {
        if (currentCrystal)
        {
            int activeCount = ActiveCrystalCount();
            // Add functionality that 1 crystal must always be active here 
            if (activeCount > 1)
            {
                SpiritDataScript.MyInstance.FlipActive(currentCrystal.crystalName);
                currentCrystal.StorePressed();
                SetTextAndButton("", false);
            }
            else
            {
                SetTextAndButton("Must Have At Least 1 Active Crystal", false);
            }

        }
    }
    public void DiscardButtonPressed()
    {
        if (currentCrystal)
        {
            // Discard a Crystal - Clear All Slots - Restock with Remaining
            currentCrystal.DiscardPressed();
            PlayerCrystalScript.MyInstance.RemoveCrystal(currentCrystal);
            ClearCrystalSlots();
            MakeCrystalSlots();
            SetTextAndButton("", false);

        }
    }

}
