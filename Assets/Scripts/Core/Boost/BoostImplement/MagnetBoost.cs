using UnityEngine;

public class MagnetBoost : TimedBoost
{
    [SerializeField] float radiusEffect;
    [SerializeField] LayerMask itemWorld;
    public override void Excute()
    {
        base.Excute();
    }
    protected override void Update()
    {
        base.Update();
        Collider2D[] arrCol = Physics2D.OverlapCircleAll(playerBoost.transform.position, radiusEffect, itemWorld);
        foreach (Collider2D col in arrCol)
        {
            if (col.TryGetComponent<CoinsWorld>(out CoinsWorld coinsWorld))
            {
                coinsWorld.CoinsWorld_OnPickupMagnetCoins(playerBoost.transform);
            }
        }

    }
    public override void Deactive()
    {
        base.Deactive();
    }
}
