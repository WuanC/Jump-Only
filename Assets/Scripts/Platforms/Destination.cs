using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour, IInteractWithPlayer
{
    public void Interact(Player player)
    {
        GameManager.Instance.PlayerWin();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            Interact(player);
        }
    }
}
