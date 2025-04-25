using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    [HideInInspector] public MyPool pool;
    public void OnDisable()
    {
        pool.AddToPool(gameObject);
    }
}