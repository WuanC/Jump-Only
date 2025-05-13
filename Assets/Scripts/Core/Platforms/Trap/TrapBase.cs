using UnityEngine;

public abstract class TrapBase : MonoBehaviour, IInteractWithPlayer
{
    public void Interact(Player player)
    {
        player.Died();
    }



    public virtual void Ready() { }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            Interact(player);
        }
    }
}
