using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour, IInteractWithPlayer
{
    bool isBeyond;
    public void Interact(Player player)
    {
        isBeyond = true;
        AudioManager.Instance.AudioSource_OnPlayerWin();
        GameManager.Instance.SlowMotionWin();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player) && !isBeyond)
        {
            Interact(player);
        }
    }
}
