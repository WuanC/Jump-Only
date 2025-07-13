using System;
using UnityEngine;

public class BoostWorld : ItemWorld
{
    [SerializeField] BoostBase boost;
    [SerializeField] SpriteRenderer sr;
    public event Action<GameObject> OnDisable;
    private void OnEnable()
    {
        if (boost != null) sr.sprite = boost.boostData.Icon;
    }
    public void SetData(BoostBase boostBase)
    {
        boost = boostBase;
        sr.sprite = boostBase.boostData.Icon;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerBoost>(out PlayerBoost playerBoost))
        {
            boost.playerBoost = playerBoost;
            Observer.Instance.Broadcast(EventId.OnPickupBoost, boost.boostData);//Quest
            OnDisable?.Invoke(gameObject);
            OnDisable = null;
            if (boost is IActivationBoost)
            {
                itemData = boost.boostData;
                base.OnTriggerEnter2D(collision);
                return;
            }
            else
            {
                if (!playerBoost.HasKey(boost.boostData.name))
                {
                    BoostBase tmp = Instantiate(boost);
                    tmp.Active();

                }
                gameObject.SetActive(false);
            }
            AudioManager.Instance.OnCollectBoost();

        }
    }
}
