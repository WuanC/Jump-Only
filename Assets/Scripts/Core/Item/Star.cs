using UnityEngine;

public class Star : MonoBehaviour, IInteractWithPlayer
{

    StarContainer container;
    private void Start()
    {
        container = GetComponentInParent<StarContainer>();
        Observer.Instance.Register(EventId.OnPlayerDied, Star_OnPlayerDied);


    }
    void Star_OnPlayerDied(object obj)
    {
        gameObject.SetActive(true);
        container.MinusStar();
    }
    public void Interact(Player player)
    {
        container.AddStar();
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            Interact(player);
        }
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerDied, Star_OnPlayerDied);
    }
}
