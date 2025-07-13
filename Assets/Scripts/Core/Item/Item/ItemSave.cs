using UnityEngine;

public class ItemSave
{
    public string ItemDataId { get; private set; }
    public int Quantity { get; private set; }
    public ItemSave(string itemDataId, int quantity)
    {
        ItemDataId = itemDataId;
        this.Quantity = quantity;
    }
}
