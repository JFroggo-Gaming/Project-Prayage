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
        // Inicjalizacja slotu, jeśli jest potrzebna
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null && itemQuantity > 0)
        {
            // Tworzenie obiektu do przeciągania
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
                TransferItemFromInventory(targetObject.GetComponent<UIInventoryCraftingSlot>());
            }
            else if (!targetObject.CompareTag("DropArea"))
            {
                DropItemOnGround();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Zdarzenie kliknięcia na slot rzemieślniczy
        EventHandler.CallCraftingSlotClickedEvent(this);
    }

    private void TransferItemFromInventory(UIInventoryCraftingSlot sourceSlot)
{
    // Przenieś informacje o przedmiocie do slotu panelu rzemieślniczego
    if (sourceSlot.itemDetails != null)
    {
        // Przypisz informacje o przedmiocie do bieżącego slotu panelu rzemieślniczego
        itemDetails = sourceSlot.itemDetails;
        itemQuantity = 1; // Ustaw ilość na 1, jeśli w panelu 3x3 ma być tylko jeden przedmiot
        UpdateSlotDisplay(); // Zaktualizuj wygląd slotu panelu rzemieślniczego

        // Oczyść slot źródłowy w panelu ekwipunku
        sourceSlot.ClearSlot();
    }
}

public void UpdateSlotDisplay()
{
    if (itemDetails != null)
    {
        inventorySlotImage.sprite = itemDetails.itemSprite;
     //   textMeshProUGUI.text = itemQuantity > 1 ? itemQuantity.ToString() : "";
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
        inventorySlotImage.sprite = null;
        textMeshProUGUI.text = "";
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
