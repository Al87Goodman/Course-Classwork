using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
    [SerializeField] private Text dateTime;
    //[SerializeField] private Text currentTown;

    /* -- Player Information to be added later -- */
    //private Text playerName;
    //private Image crystalObtained;
    //private Image badges; //?

    [SerializeField] private GameObject visuals;
    [SerializeField] private int index; // number of saved games

    public int MyIndex
    {
        get
        {
            return index;
        }
    }

    private void Awake()
    {
        //visuals.SetActive(false);
    }

    // Displays Game Information to Save/Load Menu
    public void ShowInfo(SaveData saveData)
    {
        visuals.SetActive(true);
        dateTime.text = "Date: " + saveData.MyDateTime.ToString("yyyy/MM/dd") + " - Time: " + saveData.MyDateTime.ToString("HH:mm");
        // Other information here.
    }

    public void HideInfo()
    {
        visuals.SetActive(false);
    }



}
