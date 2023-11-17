using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Camera mainCamera;
    private Canvas parentCanvas;
    private Transform parentItem;

    private Sprite originalSprite;

    private GameObject draggedItem;
    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshProUGUI;

    [SerializeField] public int constantItemQuantity = 99;  // Stała liczba przedmiotów w slocie
    [SerializeField] private Item constantItem = null;  // Referencja do przedmiotu
    [SerializeField] private int slotNumber = 0;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;

    [SerializeField] private UIInventoryBar inventoryBar = null;

    [SerializeField] public bool isSelected = false;

    [SerializeField] private GameObject inventoryTextBoxPrefab = null;

    [SerializeField] private GameObject itemPrefab = null;

    private void Start()
    {
        mainCamera = Camera.main;
        parentItem = GameObject.FindGameObjectWithTag("ItemsParentTransform").transform;
        if (constantItem != null)
        {
        InventoryManager.Instance.AddItem(InventoryLocation.player, constantItem, constantItemQuantity);
        }

        originalSprite = inventorySlotImage.sprite; // dzięki temu po "skończeniu" się itemow zachowaj oryginalny sprite slotu

    }

    /// <summary>
    /// set this iventory slot bar to be selected
    /// </summary>
    
    public void SetSelectedItem()
    {
        // clear currently highlighted item
        inventoryBar.ClearHighlightOnInventorySlots();

        // Highlight item on inventory bar
        isSelected = true;

        // Set highlighted inventory slots
        inventoryBar.SetHighlightedInventorySlots();

        // Set item selected in inventory
        InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, itemDetails.itemCode);

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null)
        {

            // Instantiate gameObject as dragged item
            draggedItem = Instantiate(inventoryBar.InventoryBarDraggedItem, inventoryBar.transform);
            draggedItem.transform.localScale = new Vector3(1f, 1f, 1f);

            // Get image for dragged item
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;

            SetSelectedItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move gameObject as dragged item
        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    private void DropSelectedItemAtMousePosition()
    {
        if (itemDetails != null && isSelected)
        {   
            
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
            // Create item from prefab at mouse position
            GameObject itemGameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = itemDetails.itemCode;
            

            // Remove item from player's inventory
            InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);

           

            // If no more item then clear selected
            if(InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, item.ItemCode) == -1)
            {
                ClearSelectedItem();
            }

        }
    }

    public void ClearSlot()
    {
    itemDetails = null;
    itemQuantity = 0;
    inventorySlotImage.sprite = originalSprite; // Przywróć oryginalny obrazek
    textMeshProUGUI.text = "";
}


    public void OnEndDrag(PointerEventData eventData)
{
    if (draggedItem != null)
    {
        Destroy(draggedItem);

        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;

        // Sprawdzanie, czy upuszczanie nastąpiło nad slotem przybornika
        if (targetObject != null && targetObject.GetComponent<UIInventorySlot>() != null)
        {   
            Debug.Log("UPUSZCZASZ NAD PANELEM");
            int toSlotNumber = targetObject.GetComponent<UIInventorySlot>().slotNumber;
            InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);
            DestroyInventoryTextBox();
            ClearSelectedItem();
        }
        // Sprawdzanie, czy upuszczanie nastąpiło nad slotem panelu rzemieślniczego
        else if (targetObject != null && targetObject.GetComponent<UIInventoryCraftingSlot>() != null)
        {   
        Debug.Log("UPUSZCZASZ NAD PANELEM 3x3");
        // Przenieś przedmiot do panelu rzemieślniczego
        InventoryManager.Instance.TransferItemToCraftingPanel(this, targetObject.GetComponent<UIInventoryCraftingSlot>());
        }

        // Upuszczanie przedmiotu na ziemię
        else if (itemDetails.canBeDropped)
        {   
            
            DropSelectedItemAtMousePosition();
        }
    }
}

    public void OnPointerClick(PointerEventData eventData)
    {
        // if left click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // if inventory slot currently selected then deselect
            if (isSelected == true)
            {
                ClearSelectedItem();
            }
            else
            {
                if (itemQuantity > 0)
                {
                    SetSelectedItem();
                }
            }
        }
    }

    public void ClearSelectedItem()
    {
        

        // Clear currently highlighted items
        inventoryBar.ClearHighlightOnInventorySlots();

        isSelected = false;

        // set no item selected in inventory
        InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);

        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Populate text box with item details
        if (itemQuantity != 0)
        {
            // Instantiate inventory text box
            inventoryBar.inventoryTextBoxGameObject = Instantiate(inventoryTextBoxPrefab, transform.position, Quaternion.identity);
            inventoryBar.inventoryTextBoxGameObject.transform.SetParent(parentCanvas.transform, false);

            UIInventoryTextBox inventoryTextBox = inventoryBar.inventoryTextBoxGameObject.GetComponent<UIInventoryTextBox>();

            // Set item type description
            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            // Populate text box
            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");

            // Set text box position according to inventory bar position
            if (inventoryBar.IsInventoryBarPositionBottom)
            {
                inventoryBar.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextBoxGameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
            }
            else
            {
                inventoryBar.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryBar.inventoryTextBoxGameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextBox();
    }

    public void DestroyInventoryTextBox()
    {
        if (inventoryBar.inventoryTextBoxGameObject != null)
        {
            Destroy(inventoryBar.inventoryTextBoxGameObject);
        }
    }
}
