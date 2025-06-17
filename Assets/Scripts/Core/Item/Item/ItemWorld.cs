using System;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public ItemDataSO item;
    public Action<GameObject> OnBeCollected;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {

            OnBeCollected?.Invoke(this.gameObject);
            OnBeCollected = null;
            GameManager.Instance.CollectGift(item.Id, 1);
            gameObject.SetActive(false);
        }

    }
}
