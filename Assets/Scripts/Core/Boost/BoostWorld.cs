using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostWorld : MonoBehaviour
{
    [SerializeField] BoostBase boost;
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
            Destroy(gameObject);
        }
    }
}
