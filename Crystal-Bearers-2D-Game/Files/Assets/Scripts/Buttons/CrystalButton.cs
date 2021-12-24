using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrystalButton : MonoBehaviour, IPointerClickHandler
{

    public IUseable MyUseable { get; set; }

    // Stack of Useable Items
    private Stack<IUseable> useables = new Stack<IUseable>();

    // Number of useables 
    private int count;

    public Button MyButton { get; private set; }

    [SerializeField]
    private Image icon;

    public Image MyIcon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }
 
    // Start is called before the first frame update
    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        // Something in hand
        if (HandScript.MyInstance.MyMoveable == null)
        {
            if (MyUseable != null)
            {
                MyUseable.Use();
            }
            // Have something in the stack 
            if (useables != null && useables.Count > 0)
            {
                useables.Peek().Use();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            // Check to see if is Useable before casting as useable
            if(HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable)
            {
                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
            }
        }

    }

    // Sets the useables on the crystal button
    public void SetUseable(IUseable useable)
    {
        if (useable is Item)
        {
            useables = InventoryScript.MyInstance.GetUseables(useable);
            count = useables.Count;
            if (InventoryScript.MyInstance.FromSlot != null)
            {
                InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
                InventoryScript.MyInstance.FromSlot = null; // need to set to null to be able to pick up other items
            }

        }
        else
        {
            this.MyUseable = useable;
        }

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        MyIcon.sprite = HandScript.MyInstance.Put().MyIcon;
        MyIcon.color = Color.white;
    }

    // Future: Crystal Count for Crystal Bar
    /*
    public void UpdateCrystalCount(Item item)
    {
        if(item is ICrystal && useables.Count > 0)
        {
        
        }
    }
    */


}
