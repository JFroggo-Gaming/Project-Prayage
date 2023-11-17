using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa obsługująca zdarzenia związane z interakcją z inventory (inwentarzem) i craftingiem (rzemiosłem).
public class EventHandler
{
    // Delegat dla zdarzenia kliknięcia w slot rzemiosła.
    public delegate void CraftingSlotClicked(UIInventoryCraftingSlot craftingSlot);
    // Zdarzenie wywoływane po kliknięciu w slot rzemiosła.
    public static event CraftingSlotClicked CraftingSlotClickedEvent;

    // Delegat dla zdarzenia aktualizacji inwentarza.
    public delegate void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList);
    // Zdarzenie wywoływane po aktualizacji inwentarza.
    public static event InventoryUpdated InventoryUpdatedEvent;

    // Metoda wywołująca zdarzenie kliknięcia w slot rzemiosła.
    public static void CallCraftingSlotClickedEvent(UIInventoryCraftingSlot craftingSlot)
    {
        CraftingSlotClickedEvent?.Invoke(craftingSlot);
    }

    // Metoda wywołująca zdarzenie aktualizacji inwentarza.
    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        InventoryUpdatedEvent?.Invoke(inventoryLocation, inventoryList);
    }
}
