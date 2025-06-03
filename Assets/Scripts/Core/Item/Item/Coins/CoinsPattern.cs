using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsPattern : MonoBehaviour
{
    [SerializeField] ItemWorld[] coins;
    private void OnDisable()
    {
        foreach(var coin in coins)
        {
            coin.gameObject.SetActive(true);
        }
    }
}
