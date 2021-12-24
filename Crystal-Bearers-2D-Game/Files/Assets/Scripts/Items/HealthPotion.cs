using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can Add A Health Potion By Going to the following in Unity:
// Assets/Create/Items/Potion
[CreateAssetMenu(fileName = "HealthPotion",menuName = "Items/HealthPotion", order=1)]
public class HealthPotion : Item, IUseable, ITossable, IPotion
{
    [SerializeField]
    private int health = 0;

    public void Use()
    {
        Debug.Log("-- USING HEALTH POTION -- ");
        // Remove From Inventory
        Remove();
    }

    public void Toss()
    {
        Debug.Log("-- TOSSING HEALTH POTION -- ");
        Remove();
    }

    // Overriding Item GetDescription()
    public override string GetDescription()
    {
        // Base needed for the parent Item information.
        return base.GetDescription() + string.Format("\nUse: Restores {0} health", health) ;
    }

    // Apply Health Potion
    public bool UsePotion(SpiritData spirit)
    {
        bool usedPotion = false;
        if (spirit.HP < spirit.maxHP)
        {
            if (spirit.HP + health >= spirit.maxHP)
            {
                spirit.HP = spirit.maxHP;
            }
            else
            {
                spirit.HP += health;
            }

            Use();
            usedPotion = true;
        }

        return usedPotion;
    }
}
