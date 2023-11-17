using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBar : MonoBehaviour
{
    [SerializeField] private Sprite blank16x16Ssprite = null;
    [SerializeField] private UIInventorySlot[] inventorySlot = null;

    [SerializeField] public GameObject inventoryTextBoxGameObject;
    public GameObject InventoryBarDraggedItem;
    private RectTransform rectTransform;

    private bool _isInventoryBarPositionBottom = true;

    public bool IsInventoryBarPositionBottom { get => _isInventoryBarPositionBottom; set => _isInventoryBarPositionBottom = value; }


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
    }
   private void OnEnable()
    {
    EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }

    private void OnDisable()
    {
    EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }


    public void ClearHighlightOnInventorySlots()
    {
        if (inventorySlot.Length > 0)
        {
            // loop through inventory slots and clear highlight sprites
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                if (inventorySlot[i].isSelected)
                {
                    inventorySlot[i].isSelected = false;
                    inventorySlot[i].inventorySlotHighlight.color = new Color(0f, 0f, 0f, 0f);
                    // Update inventory to show item as not selected
                    InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
                }
            }
        }
    }
    private void ClearInventorySlots()
{
    for (int i = 0; i < inventorySlot.Length; i++)
    {
        inventorySlot[i].inventorySlotImage.sprite = blank16x16Ssprite;
        inventorySlot[i].textMeshProUGUI.text = "";
        inventorySlot[i].itemDetails = null;
        inventorySlot[i].itemQuantity = 0;
        SetHighlightedInventorySlots(i);
    }
}

    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
{
    // Upewnij się, że aktualizujemy ekwipunek gracza
    if (inventoryLocation != InventoryLocation.player) return;

    // Iteruj przez wszystkie sloty w UI ekwipunku
    for (int i = 0; i < inventorySlot.Length; i++)
    {
        // Jeśli istnieje przedmiot w ekwipunku na tej pozycji, zaktualizuj slot
        if (i < inventoryList.Count)
        {
            InventoryItem item = inventoryList[i];
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.itemCode);

            if (itemDetails != null)
            {
                // Ustaw sprite i inne szczegóły dla slotu ekwipunku
                inventorySlot[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                inventorySlot[i].textMeshProUGUI.text = item.itemQuantity.ToString();
                inventorySlot[i].itemDetails = itemDetails;
                inventorySlot[i].itemQuantity = item.itemQuantity;
            }
        }
        else
        {
            // W przeciwnym razie wyczyść slot (jeśli nie ma przedmiotu)
            inventorySlot[i].inventorySlotImage.sprite = null; // Lub użyj sprite'a pustego slotu
            inventorySlot[i].textMeshProUGUI.text = "";
            inventorySlot[i].itemDetails = null;
            inventorySlot[i].itemQuantity = 0;
        }
    }
}


/// <summary>
///  set the selected highlight if set on all inventory position
/// </summary>
public void SetHighlightedInventorySlots()
{
    if (inventorySlot.Length >0)
    {
        // loop through inventory slots and clear highlight sprites
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            SetHighlightedInventorySlots(i);
        }
    }
}

/// <summary>
/// set the selected highlight if set on an inventory item for a given slot item positiom
/// </summary>
public void SetHighlightedInventorySlots(int itemPosition)
    {
        if (inventorySlot.Length > 0 && inventorySlot[itemPosition].itemDetails != null)
        {
            if (inventorySlot[itemPosition].isSelected)
            {
                inventorySlot[itemPosition].inventorySlotHighlight.color = new Color(1f, 1f, 1f, 1f);
 
                // Update inventory to show item as selected
                InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, inventorySlot[itemPosition].itemDetails.itemCode);
            }
        }
    }

}
