using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class UIInventoryCraftingSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshProUGUI;

    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;

    public Sprite emptySlotSprite; // Ustaw sprite dla pustego slotu w inspektorze
    private Sprite defaultSlotSprite; // Dodaj pole na domyślny sprite slotu

    private GameObject draggedItem;
    private Canvas parentCanvas;
    private Camera mainCamera;
    private Transform parentItem;
    private GameObject itemPrefab; // Przypisz prefabrykat przedmiotu w inspektorze

    private void Awake()
{
    parentCanvas = GetComponentInParent<Canvas>();
    mainCamera = Camera.main;
    parentItem = GameObject.FindGameObjectWithTag("ItemsParentTransform").transform;
    defaultSlotSprite = emptySlotSprite; // Ustaw domyślny sprite
}

    private void Start()
{
    // Ustawienie początkowego sprite'a dla pustego slotu
    inventorySlotImage.sprite = emptySlotSprite;
    defaultSlotSprite = emptySlotSprite; // Ustaw domyślny sprite
}

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null && itemQuantity > 0 && draggedItem == null)
        {
            // Tworzenie obiektu do przeciągania, jeśli draggedItem nie istnieje
            draggedItem = new GameObject("Dragged Item");
            Image draggedItemImage = draggedItem.AddComponent<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;
            draggedItemImage.raycastTarget = false;
            draggedItemImage.transform.SetParent(parentCanvas.transform, false);
            draggedItemImage.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            // Aktualizacja pozycji obiektu przeciąganego
            draggedItem.transform.position = eventData.position;
        }
    }

    private void HandleItemDrop()
{
    // Jeśli przeciągnięty na puste miejsce, usuń przedmiot z panelu rzemieślniczego
    ClearSlot();

    // Zaktualizuj slot źródłowy
    SetDefaultSprite();
    UpdateSlotDisplay();
}

public void OnEndDrag(PointerEventData eventData)
{
    if (draggedItem != null)
    {
        Destroy(draggedItem);

        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;
        if (targetObject != null && targetObject.GetComponent<UIInventoryCraftingSlot>() != null)
        {
            // Logika przeniesienia przedmiotu do nowego slotu w panelu 3x3
            TransferItemWithinCraftingPanel(targetObject.GetComponent<UIInventoryCraftingSlot>());
        }
        else if (itemDetails.canBeDropped)
        {
            // Obsługa upuszczania przedmiotu
            HandleItemDrop();
        }

        // Reset draggedItem na null po zakończeniu przeciągania
        draggedItem = null;
    }
}

    public void OnPointerDown(PointerEventData eventData)
    {
        // Zdarzenie kliknięcia na slot rzemieślniczy
        EventHandler.CallCraftingSlotClickedEvent(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        UIInventoryCraftingSlot sourceSlot = eventData.pointerDrag.GetComponent<UIInventoryCraftingSlot>();
        if (sourceSlot != null)
        {
            // Logika przenoszenia przedmiotów między slotami w panelu rzemieślniczym
            TransferItemWithinCraftingPanel(sourceSlot);
        }
    }

    private void TransferItemWithinCraftingPanel(UIInventoryCraftingSlot sourceSlot)
    {
        if (sourceSlot.itemDetails != null)
        {
            // Jeśli oba sloty mają przedmioty, zamień je miejscami
            if (itemDetails != null)
            {
                SwapItemsWith(sourceSlot);
            }
            else // W przeciwnym razie przenieś przedmiot do bieżącego slotu
            {
                MoveItemToCurrentSlot(sourceSlot);
            }
        }
    }

    private void SwapItemsWith(UIInventoryCraftingSlot otherSlot)
    {
        // Zamień miejsca dwóch przedmiotów
        ItemDetails tempItemDetails = itemDetails;
        int tempItemQuantity = itemQuantity;

        itemDetails = otherSlot.itemDetails;
        itemQuantity = otherSlot.itemQuantity;

        otherSlot.itemDetails = tempItemDetails;
        otherSlot.itemQuantity = tempItemQuantity;

        // Zaktualizuj wygląd obu slotów
        UpdateSlotDisplay();
        otherSlot.UpdateSlotDisplay();
    }

    private void MoveItemToCurrentSlot(UIInventoryCraftingSlot sourceSlot)
    {
        // Przenieś przedmiot do bieżącego slotu
        itemDetails = sourceSlot.itemDetails;
        itemQuantity = sourceSlot.itemQuantity;

        // Wyczyść źródłowy slot
        sourceSlot.ClearSlot();

        // Zaktualizuj wygląd bieżącego slotu
        UpdateSlotDisplay();
    }

    public void SetDefaultSprite()
    {
        inventorySlotImage.sprite = defaultSlotSprite;
    }

    public void UpdateSlotDisplay()
{
    
    if (itemDetails != null && itemQuantity > 0)
    {
        inventorySlotImage.sprite = itemDetails.itemSprite;
    }
    else
    {
        // Jeśli brak przedmiotu lub ilość równa zero, ustaw domyślny sprite
        inventorySlotImage.sprite = defaultSlotSprite;
    }
}


    private void ClearSlot()
    {
        itemDetails = null;
        itemQuantity = 0;
        inventorySlotImage.sprite = emptySlotSprite; // Ustawienie sprite'a dla pustego slotu
        //textMeshProUGUI.text = "";
    }

    private void DropItemOnGround()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        GameObject itemGameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
        Item item = itemGameObject.GetComponent<Item>();
        item.ItemCode = itemDetails.itemCode;
        item.Init(itemDetails.itemCode);

        // Odejmij jeden przedmiot z tego slotu
        RemoveItemFromSlot();
    }

    private void RemoveItemFromSlot()
    {
        itemQuantity--;
        if (itemQuantity == 0)
        {
            itemDetails = null;
            inventorySlotImage.sprite = emptySlotSprite;
           // textMeshProUGUI.text = "";
        }
        else
        {
            textMeshProUGUI.text = itemQuantity.ToString();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Logika obsługi kliknięcia, jeśli jest potrzebna
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Logika obsługi najechania kursorem, jeśli jest potrzebna
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Logika obsługi wyjścia kursorem, jeśli jest potrzebna
    }
}