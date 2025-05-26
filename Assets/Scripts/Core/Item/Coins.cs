using UnityEngine;

public class Coins : MonoBehaviour, IInteractWithPlayer
{
    [SerializeField] int value;
    public void Interact(Player player)
    {
        GameManager.Instance.DepositeCoins(value);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            Interact(player);
        }
    }
}
