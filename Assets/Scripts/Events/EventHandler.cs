using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{
    public delegate void CraftingSlotClicked(UIInventoryCraftingSlot craftingSlot);
    public static event CraftingSlotClicked CraftingSlotClickedEvent;

    public delegate void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList);
    public static event InventoryUpdated InventoryUpdatedEvent;

    public static void CallCraftingSlotClickedEvent(UIInventoryCraftingSlot craftingSlot)
    {
        CraftingSlotClickedEvent?.Invoke(craftingSlot);
    }

    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        InventoryUpdatedEvent?.Invoke(inventoryLocation, inventoryList);
    }
}
