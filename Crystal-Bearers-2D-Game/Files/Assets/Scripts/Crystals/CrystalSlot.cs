using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrystalSlot : MonoBehaviour
{
    [Header("Crystal/Spirit UI Attributes to Change")]
    [SerializeField] private TextMeshProUGUI crystalNumberText;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Text spiritName;
    [SerializeField] private Text spiritLevel;
    [SerializeField] private Text spiritExp;
    [SerializeField] private Text spiritHP;
    [SerializeField] private Text spiritMP;


    [Header("Crystal/Spirit Stuff")]
    public CrystalItem thisCrystal;
    public CrystalManager thisManager;
    public SpiritData thisSpirit;

    // FUNCTION FOR LOCATING SPIRIT DATA AND POPULATING SECTION

    // Setup the Crystal & Spirit Information
    public void Setup(CrystalItem newCrystal, CrystalManager newManager, SpiritData newSpirit)
    {
        thisCrystal = newCrystal;
        thisManager = newManager;
        thisSpirit = newSpirit;

        if(thisCrystal)
        {
            // Crystal/Spirit Information.
            crystalImage.sprite = thisCrystal.crystalImage;
            crystalNumberText.text = thisSpirit.level.ToString(); //thisCrystal.numberHeld.ToString();  // Want it to Be Level now
            spiritName.text = "Name: " + thisSpirit.spiritName;
            spiritLevel.text = "Level: " + thisSpirit.level.ToString();
            spiritHP.text = "HP: " + thisSpirit.HP.ToString() + "/" + thisSpirit.maxHP.ToString();
            spiritMP.text = "MP: " + thisSpirit.MP.ToString() + "/" + thisSpirit.maxMP.ToString();
            spiritExp.text = "EXP: " + thisSpirit.totalExp.ToString();


            if (thisCrystal.isActive)
            {
                GetComponent<Image>().color = new Color(0, 255, 101);
            }
        }
    }

    // pass the Spirit Also?
    public void ClickedOn()
    {
        if(thisCrystal)
        {
            // Send information to the Crystal Manager When the Slot is Clicked ON
            thisManager.SetupDescriptionAndButton(thisCrystal.crystalDescription,thisCrystal.isActive, thisCrystal);
        }
    }

}
