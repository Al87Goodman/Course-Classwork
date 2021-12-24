//REFERENCE: inScope Studios - https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandScript : MonoBehaviour
{

    // HandScript Singleton
    private static HandScript instance;
    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }

    public IMoveable MyMoveable { get; set; }

    private Image icon;

    // offset for dragging icon
    [SerializeField]
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // Moves the icon around with the mouse
        icon.transform.position = Input.mousePosition + offset;

        // Delete Items in Hand
        DeleteItem();
    }

    // Access icon (MyIcon) through the IMoveable Interface
    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }

    // Put Item
    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0); //transparent

        return tmp;
    }

    // Drop Item
    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
    }

    // Delete Item
    private void DeleteItem()
    {
        // Carrying something in hand , press first mouse button, not hovering over any UI elements.
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null)
        {
            if (MyMoveable is Item)
            {
                Item item = (Item)MyMoveable;
                if (item.MySlot != null)
                {
                    item.MySlot.Clear();
                }
            }
            Drop();

            // remove reference
            InventoryScript.MyInstance.FromSlot = null;
        }
    }
}
