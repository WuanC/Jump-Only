using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public ItemDataSO item;
    public Action<GameObject> OnBeCollected;
    public void Setup(Action<GameObject> action)
    {
        OnBeCollected = action;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            if (OnBeCollected != null)
            {
                OnBeCollected?.Invoke(this.gameObject);
                OnBeCollected = null;
            }
            GameManager.Instance.CollectGift(item.Id, 1);
            gameObject.SetActive(false);
        }
        
    }
}
