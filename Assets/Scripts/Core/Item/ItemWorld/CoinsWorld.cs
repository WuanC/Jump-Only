using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsWorld : ItemWorld
{
    bool canMove;
    [SerializeField] float speed;
    public void CoinsWorld_OnPickupMagnetCoins(Transform playerTrans)
    {
        if (playerTrans != null)
        {
            canMove = true;
            StartCoroutine(ComeClosePlayer(playerTrans));
        }

    }
    IEnumerator ComeClosePlayer(Transform playerTransform)
    {
        float timeMove = Vector2.Distance(playerTransform.position, transform.position) / speed;
        float t = 0;
        while(t < timeMove)
        {
            transform.position = Vector2.Lerp(transform.position, playerTransform.position, t / timeMove);
            t += Time.deltaTime;
            yield return null;
        }
        canMove = false;
    }
}
