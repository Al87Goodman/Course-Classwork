using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can Add A Health Potion By Going to the following in Unity:
// Assets/Create/Items/Potion
[CreateAssetMenu(fileName = "PoisonPotion", menuName = "Items/PoisonPotion", order = 2)]
public class PoisonPotion : Item, IUseable, ITossable, IPotion
{
    [SerializeField] private bool poison = false;

    public void Use()
    {

        Debug.Log("-- USING POISON POTION -- ");
        // Remove From Inventory
        Remove();
    }

    public void Toss()
    {
        Debug.Log("-- TOSSING POISON POTION -- ");
        Remove();
    }

    // Overriding Item GetDescription()
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\nUse: Removes Poison");
    }

    // Apply Health Potion
    public bool UsePotion(SpiritData spirit)
    {
        bool usedPotion = false;

        // Check if Spirit Poisoned -- Currently does not have the capability to tell if poisoned or not.

        return usedPotion;
    }

}