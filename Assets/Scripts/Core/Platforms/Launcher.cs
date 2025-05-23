using UnityEngine;

public class Launcher : MonoBehaviour, IInteractWithPlayer
{
    [SerializeField] float launchForce;
    public void Interact(Player player)
    {
        player.transform.position = transform.position;
        Vector2 direction = transform.up * launchForce;
        player.AddForceToPlayer(direction.x, direction.y);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            Interact(player);
        }
    }
}
