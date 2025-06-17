using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject playerDeathEffect;

    public void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, EffectManager_SpawnEffectDestroy);
        Observer.Instance.Register(EventId.OnSpawnEffect, EffectManager_SpawnEffectDestroy);
    }
    public void EffectManager_SpawnEffectDestroy(object obj)
    {
        Vector3 postition = (Vector3)obj;
        if (playerDeathEffect != null)
        {
            GameObject effect = MyPoolManager.Instance.GetFromPool(playerDeathEffect, transform);
            effect.transform.position = postition;
        }
    }
    public void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerDied, EffectManager_SpawnEffectDestroy);
        Observer.Instance.Unregister(EventId.OnSpawnEffect, EffectManager_SpawnEffectDestroy);
    }
}
