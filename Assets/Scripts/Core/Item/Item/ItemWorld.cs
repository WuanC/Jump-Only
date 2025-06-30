using System;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public ItemDataSO itemData;
    public Action<GameObject> OnBeCollected;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            OnBeCollected?.Invoke(this.gameObject);
            OnBeCollected = null;
            Item item = new Item(itemData, 1);
            Inventory.Instance.AddItem(item);
            gameObject.SetActive(false);
        }

    }
}
