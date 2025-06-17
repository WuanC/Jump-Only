using UnityEngine;

[CreateAssetMenu(fileName = "New gift", menuName = "SO/Gift")]
public class Gift : ScriptableObject
{
    [SerializeField] GiftWrapper[] giftWappers;
    public GiftWrapper[] GiftWrapper => giftWappers;
    public void CollectGift()
    {
        for (int i = 0; i < giftWappers.Length; i++)
        {
            GameManager.Instance.CollectGift(giftWappers[i].data.Id, giftWappers[i].quantity);
        }
    }

}
