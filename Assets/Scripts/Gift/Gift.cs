using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New gift", menuName = "SO/Gift")]
public class Gift : ScriptableObject
{
    [field: SerializeField] public string GiftId { get; private set; }
    [SerializeField] GiftWrapper[] giftWappers;
    public GiftWrapper[] GiftWrapper => giftWappers;

    public void CollectGift()
    {
        for (int i = 0; i < giftWappers.Length; i++)
        {
            Inventory.Instance.AddItem(new Item(giftWappers[i].data, giftWappers[i].quantity));
        }
    }

}
