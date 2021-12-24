using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSelection : MonoBehaviour
{

    // Crystal Selection and Game State (New Game or Not)
    [Header("Crystal Selection and Game State")]
    [SerializeField] private GameObject selectionPanel; // Content
    [SerializeField] private Text descriptionText;
    public GameInfo gameInfo;

    // Buttons for displaying specific information
    [Header("Buttons")]
    [SerializeField] private GameObject buttonBegin;
    [SerializeField] private GameObject buttonRed;
    [SerializeField] private GameObject buttonBlue;
    [SerializeField] private GameObject buttonGreen;
    [SerializeField] private GameObject buttonOrange;
    [SerializeField] private GameObject buttonYellow;


    private string infoText = "";
    //private string crystalName = "";
    private string color = "";
    public bool gameState;

    // Start is called before the first frame update

    private void Awake()
    {
        gameState = gameInfo.NewGame;
        Debug.Log("GAME STATE: " + gameState);
        // if !beginning of game
        if (gameState)
        {
            // Pause Animations & Open Up the Selection Panel
            Time.timeScale = 0f; 
            selectionPanel.SetActive(true);

            // Make Sure Buttons are Active (except the Begin Button)
            buttonRed.SetActive(true);
            buttonBlue.SetActive(true);
            buttonGreen.SetActive(true);
            buttonOrange.SetActive(true);
            buttonYellow.SetActive(true);
            buttonBegin.SetActive(false);
            descriptionText.text = "Select a Color Before Starting Your Journey.";
            gameInfo.NewGame = false;
        }
        else
        {
            selectionPanel.SetActive(false);
        }

    }
    
    private void OnEnable()
    {
        //buttonBegin.SetActive(false);

    }

    public void DisplayText(string color)
    {
        // Display Crystal Color Prompt
        infoText = string.Format("Begin Your Journey with the {0} Crystal?\n\nAfter selection check your Crystal Bag by pressing the ESC key ", color);
        descriptionText.text = infoText;
        buttonBegin.SetActive(true);
    }

    public void ClickBegin()
    {
        gameInfo.NewGame = false;
        switch (color)
        {
            case "Red":
                PlayerCrystalScript.MyInstance.AddStarterCrystal("Sonic");
                break;
            case "Blue":
                PlayerCrystalScript.MyInstance.AddStarterCrystal("Spike");
                break;
            case "Green":
                PlayerCrystalScript.MyInstance.AddStarterCrystal("Stabagator");
                break;
            case "Orange":
                PlayerCrystalScript.MyInstance.AddStarterCrystal("Ham-Ham");
                break;
            case "Yellow":
                PlayerCrystalScript.MyInstance.AddStarterCrystal("Boom");
                break;
            default:
                break;
        }

        // Set New Game Object to False
        gameInfo.NewGame = false;

        // Deactive the Selecion Panel & Start Up the Animation.
        selectionPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Corresponds to a particular crystal.
    public void ClickRed()
    {
        color = "Red";
        DisplayText(color);
    }
    public void ClickBlue()
    {
        color = "Blue";
        DisplayText(color);
    }
    public void ClickGreen()
    {
        color = "Green";
        DisplayText(color);
    }
    public void ClickOrange()
    {
        color = "Orange";
        DisplayText(color);
    }
    public void ClickYellow()
    {
        color = "Yellow";
        DisplayText(color);
    }








}
