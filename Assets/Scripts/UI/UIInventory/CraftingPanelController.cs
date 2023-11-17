using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CraftingPanelController : MonoBehaviour
{
    public GameObject slotPrefab;
    public GridLayoutGroup gridLayoutGroup;
    public int maxSlots = 12; // Maksymalna liczba slotów
    public int minSlots = 9;  // Minimalna liczba slotów

    private int currentSlots = 9; // Początkowa liczba slotów

    public void AddSlot()
    {
        if (currentSlots < maxSlots)
        {
            currentSlots++;
            UpdateGridLayout();
        }
    }

    public void RemoveSlot()
    {
        if (currentSlots > minSlots)
        {
            currentSlots--;
            UpdateGridLayout();
        }
    }

    private void UpdateGridLayout()
    {
        // Dodaj lub usuń sloty w zależności od aktualnej liczby slotów
        while (gridLayoutGroup.transform.childCount < currentSlots)
        {
            Instantiate(slotPrefab, gridLayoutGroup.transform);
        }

        while (gridLayoutGroup.transform.childCount > currentSlots)
        {
            DestroyImmediate(gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).gameObject);
        }

        foreach (Transform child in gridLayoutGroup.transform)
        {
            UIInventoryCraftingSlot craftingSlot = child.GetComponent<UIInventoryCraftingSlot>();
            if (craftingSlot != null)
            {
                // Dodaj obsługę Drag & Drop dla slotów rzemieślniczych
                AddDragAndDrop(craftingSlot);
            }
        }
    }

    private void AddDragAndDrop(UIInventoryCraftingSlot craftingSlot)
{
    EventTrigger eventTrigger = craftingSlot.gameObject.GetComponent<EventTrigger>();
    if (eventTrigger == null)
    {
        eventTrigger = craftingSlot.gameObject.AddComponent<EventTrigger>();
    }

    // Remove existing triggers to avoid duplicates
    eventTrigger.triggers.Clear();

    // Add event triggers for drag and drop
    EventTrigger.Entry entryBeginDrag = new EventTrigger.Entry();
    entryBeginDrag.eventID = EventTriggerType.BeginDrag;
    entryBeginDrag.callback.AddListener((eventData) => craftingSlot.OnBeginDrag((PointerEventData)eventData));
    eventTrigger.triggers.Add(entryBeginDrag);

    EventTrigger.Entry entryDrag = new EventTrigger.Entry();
    entryDrag.eventID = EventTriggerType.Drag;
    entryDrag.callback.AddListener((eventData) => craftingSlot.OnDrag((PointerEventData)eventData));
    eventTrigger.triggers.Add(entryDrag);

    EventTrigger.Entry entryEndDrag = new EventTrigger.Entry();
    entryEndDrag.eventID = EventTriggerType.EndDrag;
    entryEndDrag.callback.AddListener((eventData) => craftingSlot.OnEndDrag((PointerEventData)eventData));
    eventTrigger.triggers.Add(entryEndDrag);
}

    void Start()
    {
        UpdateGridLayout();
    }
}
