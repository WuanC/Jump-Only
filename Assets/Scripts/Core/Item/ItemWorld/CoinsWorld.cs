using System.Collections;
using UnityEngine;

public class CoinsWorld : ItemWorld
{
    private Vector2 startPos;
    [SerializeField] float speed;
    public void CoinsWorld_OnPickupMagnetCoins(Transform playerTrans)
    {
        if (playerTrans != null)
        {
            startPos = transform.position;
            StartCoroutine(ComeClosePlayer(playerTrans));
        }

    }
    IEnumerator ComeClosePlayer(Transform playerTransform)
    {

        float timeMove = Vector2.Distance(playerTransform.position, transform.position) / speed;
        float t = 0;
        while (t < timeMove)
        {
            if (playerTransform == null || !playerTransform.gameObject.activeSelf)
            {
                transform.position = startPos;
                yield break;
            }
            transform.position = Vector2.Lerp(transform.position, playerTransform.position, t / timeMove);
            t += Time.deltaTime;
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
