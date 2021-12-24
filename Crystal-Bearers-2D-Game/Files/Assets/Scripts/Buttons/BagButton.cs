using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    private Bag bag = null;

    [SerializeField]
    private Sprite full = null, empty = null;

    // index the bags
    [SerializeField]
    private int bagIndex;

    // Changes the Canvas Bag Bar to display empty bag image or non-empty bag image
    public Bag MyBag
    {
        get
        {
            return bag;
        }

        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }

            bag = value;
        }
    }

    public int MyBagIndex { get => bagIndex; set => bagIndex = value; }

    // Open/Close Bag or Dequip
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            // carrying something and it is a bag.
            if (InventoryScript.MyInstance.FromSlot != null && HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Bag)
            {
                // If bag is equipped on this button, then swap it with carrying in hand
                if (MyBag != null)
                {
                    InventoryScript.MyInstance.SwapBags(MyBag, HandScript.MyInstance.MyMoveable as Bag);
                }
                // place in empty bag button slot
                else
                {
                    Bag tmp = (Bag)HandScript.MyInstance.MyMoveable;
                    tmp.MyBagButton = this;
                    tmp.Use();
                    MyBag = tmp;
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
            // Dequip When Holding Shift + Left Click
            else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                HandScript.MyInstance.TakeMoveable(MyBag);
            }
            // Open/Close Bag
            else if (bag != null)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    // Remove and Unbind Bag
    public void RemoveBag()
    {
        InventoryScript.MyInstance.RemoveBag(MyBag);
        //Bag Button is no longer bound to any particular bag
        MyBag.MyBagButton = null;
        // place items in bag that is being removed into available bags
        foreach (Item item in MyBag.MyBagScript.GetItems())
        {
            InventoryScript.MyInstance.AddItem(item);
        }
        MyBag = null;
    }
}
