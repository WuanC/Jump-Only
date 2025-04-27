using UnityEngine;

public abstract class TrapBase : MonoBehaviour, IInteractWithPlayer
{
    public void Interact(Player player)
    {
        player.Died();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            //FindObjectOfType<LogError>().settext(gameObject.name);
            Interact(player);
        }
    }
}
