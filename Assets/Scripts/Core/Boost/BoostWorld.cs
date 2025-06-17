using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostWorld : MonoBehaviour
{
    [SerializeField] BoostBase boost;
    [SerializeField] SpriteRenderer sr;
    public event Action<GameObject> OnDisable;
    private void OnEnable()
    {
        if (boost != null) sr.sprite = boost.boostData.icon;
    }
    public void SetData(BoostBase boostBase)
    {
        boost = boostBase;
        sr.sprite = boostBase.boostData.icon;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerBoost>(out PlayerBoost playerBoost))
        {
            boost.playerBoost = playerBoost;
            if (!playerBoost.HasKey(boost.boostData.name))
            {
                BoostBase tmp = Instantiate(boost);
                tmp.Active();
            }
            Observer.Instance.Broadcast(EventId.OnPickupBoost, boost.boostData);
            gameObject.SetActive(false);
            OnDisable?.Invoke(gameObject);
            OnDisable = null;
        }
    }
}
