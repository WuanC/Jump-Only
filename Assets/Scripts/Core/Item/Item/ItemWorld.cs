using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public ItemDataSO item;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            GameManager.Instance.CollectGift(item.Id, 1);
            gameObject.SetActive(false);
        }
        
    }
}
