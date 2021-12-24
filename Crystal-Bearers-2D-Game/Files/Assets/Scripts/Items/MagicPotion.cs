using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can Add A Health Potion By Going to the following in Unity:
// Assets/Create/Items/Potion
[CreateAssetMenu(fileName = "MagicPotion", menuName = "Items/MagicPotion", order = 3)]
public class MagicPotion : Item, IUseable, ITossable, IPotion
{
    [SerializeField]
    private int magic = 15;

    public void Use()
    {
        Debug.Log("-- USING MAGIC POTION -- ");

        // Remove From Inventory
        Remove();
    }

    public void Toss()
    {
        Debug.Log("-- TOSSING MAGIC POTION -- ");
        Remove();
    }

    // Overriding Item GetDescription()
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\nUse: Restores {0} magic", magic);
    }

    // Apply Magic Potion
    public bool UsePotion(SpiritData spirit)
    {
        bool usedPotion = false;
        if (spirit.MP < spirit.maxMP)
        {
            if (spirit.MP + magic >= spirit.maxMP)
            {
                spirit.MP = spirit.maxMP;
            }
            else
            {
                spirit.MP += magic;
            }

            Use();
            usedPotion = true;
        }

        return usedPotion;
    }

}