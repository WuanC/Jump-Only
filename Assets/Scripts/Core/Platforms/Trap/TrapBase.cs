using System;
using UnityEngine;

public abstract class TrapBase : MonoBehaviour, IInteractWithPlayer
{
    public event Action<GameObject> OnTrapDisable;
    public bool cantDestroy;
    public bool isIndividual;
    public void Interact(Player player)
    {
        player.Died();
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            Interact(player);
        }
        else if(collision.gameObject.TryGetComponent<Rocket>(out Rocket trapBase))
        {
            Debug.Log("roclet");
        }
    }
    public void DestroySelf(bool spawnEffect = true)
    {
        OnTrapDisable?.Invoke(this.gameObject);
        OnTrapDisable = null;
        if(spawnEffect) Observer.Instance.Broadcast(EventId.OnSpawnEffect, transform.position);
        gameObject.SetActive(false);
    }
    
}
