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
    }

    private void Start()
{
    // Ustawienie początkowego sprite'a dla pustego slotu
    inventorySlotImage.sprite = emptySlotSprite;
    defaultSlotSprite = inventorySlotImage.sprite; // Ustaw domyślny sprite
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
            else if (!targetObject.CompareTag("DropArea"))
            {
                DropItemOnGround();
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
        // Przypisz informacje o przedmiocie do bieżącego slotu panelu rzemieślniczego
        itemDetails = sourceSlot.itemDetails;
        itemQuantity = sourceSlot.itemQuantity;
        
        // Oczyść slot źródłowy w panelu rzemieślniczym
        sourceSlot.ClearSlot();
        sourceSlot.UpdateSlotDisplay(); // Dodaj tę linię do aktualizacji źródłowego slotu

        // Aktualizuj wyświetlanie dla bieżącego slotu
        UpdateSlotDisplay();
    }
}


public void SetDefaultSprite()
    {
        inventorySlotImage.sprite = defaultSlotSprite;
    }

public void UpdateSlotDisplay()
    {
        if (itemDetails != null)
        {
            inventorySlotImage.sprite = itemDetails.itemSprite;
          //  textMeshProUGUI.text = itemQuantity > 1 ? itemQuantity.ToString() : "";
        }
        else
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        itemDetails = null;
        itemQuantity = 0;
        inventorySlotImage.sprite = emptySlotSprite; // Ustawienie sprite'a dla pustego slotu
       // textMeshProUGUI.text = "";
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
            inventorySlotImage.sprite = null;
            textMeshProUGUI.text = "";
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
