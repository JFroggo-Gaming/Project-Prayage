using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> itemDetailsDictionary;  // Stores item code and corresponding item details
    private int[] selectedInventoryItem; // the index of the array is the inventory list, and the value is the item code
    public List<InventoryItem>[] inventoryLists;
    [HideInInspector] public int[] inventoryListCapacityInArray; // the index of the array is the inventory list (from the InventoryLocation enum), and the value is the capacity of the inventory list
    [SerializeField] private SO_ItemList itemList = null; // is a reference to a Scriptable Object called SO_ItemList, which holds a list of ItemDetails.

    protected override void Awake()
    {
        base.Awake();
        // Create Inventory Lists
        CreateInventoryLists();
        // Create item details dictionary
        CreateItemDetailsDictionary();
        // Initialize selected inventory array
        selectedInventoryItem = new int[(int)InventoryLocation.count];
        for (int i = 0; i < selectedInventoryItem.Length; i++)
        {
            selectedInventoryItem[i] = -1;
        }
    }

    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];
        for (int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }
        // Initialize inventory list capacity array
        inventoryListCapacityInArray = new int[(int)InventoryLocation.count];
        // Initialize player inventory list capacity
        inventoryListCapacityInArray[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
    }

    // Populates the itemDetailsDictionary from the scriptable object items list
    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();
        foreach (ItemDetails itemDetails in itemList.itemDetails)
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }
    private void AddSingleItem(InventoryLocation inventoryLocation, Item item)
{
    // Sprawdź, czy istnieje już przedmiot o danym kodzie w ekwipunku
    int itemPosition = FindItemInInventory(inventoryLocation, item.ItemCode);

    // Jeśli przedmiot już istnieje, zwiększ jego ilość
    if (itemPosition != -1)
    {
        AddItemAtPosition(inventoryLists[(int)inventoryLocation], item.ItemCode, itemPosition);
    }
    // Jeśli przedmiot nie istnieje w ekwipunku, dodaj go na końcu listy
    else
    {
        AddItemAtPosition(inventoryLists[(int)inventoryLocation], item.ItemCode);
    }
}
    /// <summary>
    /// Add an item to the inventory list for the inventoryLocation
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete)
    {
        AddItem(inventoryLocation, item);

        Destroy(gameObjectToDelete);
    }

    /// <summary>
    /// Add an item to the inventory list for the inventoryLocation
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, Item item, int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            AddSingleItem(inventoryLocation, item);
        }

        // Jeśli podano ilość, zaktualizuj zdarzenie, że magazyn został zaktualizowany
        if (quantity > 0)
        {
            EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
        }
    }


    /// <summary>
    /// Add item to the end of the inventory
    /// </summary>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);
    }

    /// <summary>
    /// Add item to position in the inventory
    /// </summary>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = inventoryList[position];
        inventoryItem.itemQuantity++;
        inventoryList[position] = inventoryItem;
    }

    /// <summary>
    /// Swap item at fromItem index with item at toItem index in inventoryLocation inventory list
    /// </summary>
    public void SwapInventoryItems(InventoryLocation inventoryLocation, int fromItem, int toItem)
    {
        // if fromItem and toItem index and within the bounds of the list, not the same, and greater than or equal to zero
        if (fromItem < inventoryLists[(int)inventoryLocation].Count && toItem < inventoryLists[(int)inventoryLocation].Count
            && fromItem != toItem && fromItem >= 0 && toItem >= 0)
        {
            InventoryItem fromInventoryItem = inventoryLists[(int)inventoryLocation][fromItem];
            InventoryItem toInventoryItem = inventoryLists[(int)inventoryLocation][toItem];

            inventoryLists[(int)inventoryLocation][toItem] = fromInventoryItem;
            inventoryLists[(int)inventoryLocation][fromItem] = toInventoryItem;

            // Send event that inventory has been updated
            EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
        }
    }

    /// <summary>
    /// Find if an itemCode is already in the inventory. Returns the item position in the inventory list, or -1 if the item is not in the inventory
    /// </summary>
    public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].itemCode == itemCode)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Returns the itemDetails (from the SO_ItemList) for the itemCode, or null if the item code doesn't exist
    /// </summary>
    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;

        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
        {
            return itemDetails;
        }
        else
        {
            return null;
        }
    }

public string GetItemTypeDescription(ItemType itemType)
{
    string itemTypeDescription;
    switch (itemType)
    {
        case ItemType.IntencjaZlota:
            itemTypeDescription = Settings.IntencjaZlota;
            break;

        case ItemType.IntencjaPoswiecenia:
            itemTypeDescription = Settings.IntencjaPoswiecenia;
            break;

        case ItemType.IntencjaPlodnosci:
            itemTypeDescription = Settings.IntencjaPlodnosci;
            break;

        case ItemType.IntencjaWiary:
            itemTypeDescription = Settings.IntencjaWiary;
            break;

        case ItemType.IntencjaWiedzy:
            itemTypeDescription = Settings.IntencjaWiedzy;
            break;

        case ItemType.IntencjaPomyslnosci:
            itemTypeDescription = Settings.IntencjaPomyslnosci;
            break;

        case ItemType.IntencjaBlogoslawienstwa:
            itemTypeDescription = Settings.IntencjaBlogoslawienstwa;
            break;
        case ItemType.IntencjaPrzyjaciela:
            itemTypeDescription = Settings.IntencjaPrzyjaciela;
            break;
        case ItemType.IntencjaRelikwiarzu:
            itemTypeDescription = Settings.IntencjaRelikwiarzu;
            break;
        case ItemType.ZarliwejModlitwy:
            itemTypeDescription = Settings.ZarliwejModlitwy;
            break;
        case ItemType.MajestatycznejModlitwy:
            itemTypeDescription = Settings.MajestatycznejModlitwy;
            break;
        case ItemType.CholerycznejModlitwy:
            itemTypeDescription = Settings.CholerycznejModlitwy;
            break;
        case ItemType.BiernejModlitwy:
            itemTypeDescription = Settings.BiernejModlitwy;
            break;
        

        default:
            itemTypeDescription = itemType.ToString();
            break;
    }
    return itemTypeDescription;
}

    // Remove an item from the inventory and create a gameObject at the possition this item was dropped
    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        // Check if inventory already contains the item
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            RemoveItemAtPosition(inventoryList, itemCode, itemPosition);
        }

        // Send event that inventory has been updated
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryList);
    }

    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = inventoryList[position];
        inventoryItem.itemQuantity--;

        if (inventoryItem.itemQuantity > 0)
        {
            inventoryList[position] = inventoryItem;
        }
        else
        {
            inventoryList.RemoveAt(position);
        }
    }

    /// <summary>
    /// Set the selected inventory item for inventoryLocation to itemCode
    /// </summary>
    public void SetSelectedInventoryItem(InventoryLocation inventoryLocation, int itemCode)
    {
        selectedInventoryItem[(int)inventoryLocation] = itemCode;
    }

    /// <summary>
    /// Clear the selected inventory item for inventoryLocation
    /// </summary>
    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        selectedInventoryItem[(int)inventoryLocation] = -1;
    }

    /* private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    {   
        foreach (InventoryItem inventoryItem in inventoryList)
        {
            Debug.Log("Item Description: " + GetItemDetails(inventoryItem.itemCode).itemDescription + " Item Quantity: " + inventoryItem.itemQuantity);
        }
        Debug.Log("*****************************************************");
    } */
}
