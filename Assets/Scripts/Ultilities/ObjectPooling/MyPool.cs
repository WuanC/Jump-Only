using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPool : MonoBehaviour
{
    private Stack<GameObject> stack = new Stack<GameObject>();
    private GameObject prefab;

    private Transform parent;
    private GameObject tmpObject;
    private ReturnToPool returnToPool;
    public MyPool(GameObject baseObject, Transform parent = null)
    {
        this.prefab = baseObject;
        this.parent = parent;
    }
    public GameObject Get()
    {
        if (stack.Count > 0)
        {
            tmpObject = stack.Pop();
            tmpObject.SetActive(true);
            return tmpObject;
        }
        tmpObject = Instantiate(prefab, parent);
        returnToPool = tmpObject.AddComponent<ReturnToPool>();
        returnToPool.pool = this;
        return tmpObject;

    }
    public void AddToPool(GameObject obj)
    {
        stack.Push(obj);
    }
}