using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPoolManager : Singleton<MyPoolManager>
{
    private Dictionary<GameObject, MyPool> pools = new Dictionary<GameObject, MyPool>();
    public GameObject GetFromPool(GameObject baseObject, Transform parent)
    {
        if (!pools.ContainsKey(baseObject))
        {
            pools.Add(baseObject, new MyPool(baseObject, parent));
        }
        return pools[baseObject].Get();
    }
}