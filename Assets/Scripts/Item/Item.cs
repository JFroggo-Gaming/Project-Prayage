using UnityEngine;

public class Item : MonoBehaviour
{
    [ItemCodeDescription]
    [SerializeField]
    private int _itemCode;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D savedColliderData; // Dla 3D użyj BoxCollider

    public int ItemCode { get { return _itemCode; } set { _itemCode = value; } }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        savedColliderData = GetComponent<BoxCollider2D>(); // Dla 3D użyj GetComponent<BoxCollider>()
    }

    private void Start()
    {
        if (ItemCode != 0)
        {
            Init(ItemCode);
        }
    }

    public void Init(int itemCodeParam)
    {
        if (itemCodeParam != 0)
        {
            ItemCode = itemCodeParam;
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode);
            spriteRenderer.sprite = itemDetails.itemSprite;
        }
    }

    // Handle mouse click to pick up the item
    private void OnMouseDown()
    {
        // Zapisz dane BoxCollider
        SaveColliderData();
        
        // Logic to pick up the item and add it to the inventory
        InventoryManager.Instance.AddItem(InventoryLocation.player, this);

        // Usuń obiekt z sceny
        Destroy(gameObject);

    }
    // Zapisz aktualny stan BoxCollider
    private void SaveColliderData()
    {
        if (savedColliderData == null)
        {
            savedColliderData = GetComponent<BoxCollider2D>(); // Dla 3D użyj GetComponent<BoxCollider>()
        }

        // Zapisz istotne właściwości collidera, np. rozmiar i pozycję
        // Możesz też zapisać inne właściwości, jeśli są potrzebne
    }

    // Przywróć stan BoxCollider po reinstancjonowaniu
    public void RestoreColliderState()
    {
        var collider = GetComponent<BoxCollider2D>(); // Dla 3D użyj GetComponent<BoxCollider>()
        if (collider != null && savedColliderData != null)
        {
            collider.size = savedColliderData.size;
            collider.offset = savedColliderData.offset;
            // Przywróć inne właściwości, jeśli jest to potrzebne
        }
    }
}
