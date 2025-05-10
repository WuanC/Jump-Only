using UnityEngine;

public class BoostSpeedItem : MonoBehaviour, IInteractWithPlayer
{
    [SerializeField] float duration;
    public void Interact(Player player)
    {
        player.transform.position = transform.position;
        Vector2 direction = transform.up;
        Observer.Instance.Broadcast(EventId.OnEnterJumpPad, (duration, new Vector2(transform.up.x, transform.up.y)));
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
