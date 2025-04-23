using UnityEngine;

public abstract class TrapBase : MonoBehaviour, IInteractWithPlayer
{
    public void Interact(Player player)
    {
        player.Died();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            Interact(player);
        }
    }
}
