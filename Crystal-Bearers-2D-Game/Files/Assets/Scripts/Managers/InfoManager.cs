using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    // Information Display
    [Header("Information Menu")]
    [SerializeField] private GameObject infoPanelContent; // Content
    [SerializeField] private Text descriptionText;

    // Buttons for displaying specific information
    [Header("Buttons")]
    [SerializeField] private GameObject controlsButton;
    [SerializeField] private GameObject mapButton;
    [SerializeField] private GameObject devButton;

    private string infoText = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        controlsButton.SetActive(true);
        mapButton.SetActive(true);
        devButton.SetActive(true);
        string infoText = "Click a Button to See Information";
        DisplayText(infoText);
    }


    public void DisplayText(string infoText)
    {
        // Empty Description
        descriptionText.text = infoText;
    }

    // Game Control Information
    public void DisplayControls()
    {
       infoText = "INVENTORY\n" +
            "  - Open/Close All Bags: B Key\n" +
            "  - Open/Close Single Bag: Left Click Bag Icon\n" +
            "  - Dequip a Bag: Left Click Bag Icon + SHIFT + Drag Empty Slot\n" +
            "  - Item Use Menu: Right Click Item Icon\n" +
            "\nVENDOR\n" +
            "  - Talk with Vendor: Walk to Counter, Press Space Bar\n" +
            "  - Purchase Item: Left Click Item Icon\n" +
            "\nBATTLE\n" +
            "  - Physical Attack: Left Click Sword Icon\n" +
            "  - Magical Attack: Left Click Staff Icon\n" +
            "  - Run: Left Click Boots Icon\n";

        DisplayText(infoText);
    }

    // Game Map Text Description (Better with Image, but Not Enough Time ...So Text it is!)
    public void DisplayMap()
    {

        infoText = "MAP\n" +
            "  - 5 Towns Connected by Wild Areas:\n" +
            "    1. Rose City (Capital)\n" +
            "    2. Labyrinth Woods (Forest Route)\n" +
            "    3. Lilypad Port (Coastal Route)\n" +
            "    4. Marigold Municipality (Plains Route)\n" +
            "    5. Cacti Town (Desert Route)\n" +
            "  - Town Bosses:\n" +
            "    1. Ruby (Capital)\n" +
            "    2. Hagatha (Forest)\n" +
            "    3. Marina (Coastal)\n" +
            "    4. Arborous (Plains)\n" +
            "    5. Toxitra (Desert)\n";

        DisplayText(infoText);
    }

    // Display Developer/Debug HotKeys
    public void DisplayDev()
    {
        infoText = "DEVELOPER/TESTER HOT KEYS:\n" +
            "  - Add a Crystal: X Key\n" +
            "  - Add Potions (Health, Magic, Poison): H, M, P Keys\n";

        DisplayText(infoText);

    }






}
