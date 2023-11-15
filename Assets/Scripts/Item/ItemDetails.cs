using UnityEngine;

[System.Serializable]
public class ItemDetails
{
   public int itemCode;
   public ItemType itemType;
   public string itemDescription;
   public Sprite itemSprite;
   public string itemLongDescription;
   public bool canBePickedUp;
   public bool canBeDropped;
}
