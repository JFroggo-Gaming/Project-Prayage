using UnityEngine;
using UnityEngine.UI;

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
    }

    void Start()
    {
        UpdateGridLayout();
    }
}
