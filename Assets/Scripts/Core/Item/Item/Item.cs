using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemDataSO itemData;
    public int quantity;

    public Item(ItemDataSO itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
    }
    public virtual void AddQuantity(int quantity)
    {
        this.quantity = quantity;
    }
}

