using UnityEngine;

public class Star : MonoBehaviour, IInteractWithPlayer
{

    StarContainer container;
    private void Start()
    {
        container = GetComponentInParent<StarContainer>();
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
            Debug.Log("interact");
            Interact(player);
        }
    }
}
