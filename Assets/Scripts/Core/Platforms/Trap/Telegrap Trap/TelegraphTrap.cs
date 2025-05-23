using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class TelegraphTrap : MonoBehaviour
{
    SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnDisable()
    {
        Color color = sr.color;
        color.a = 1f;
        sr.color = color;
        sr.DOKill();
    }
    public void Initial(Action callbackWhenEnd, float timeWarning)
    {
        sr.DOFade(0.3f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        StartCoroutine(WaitForDisable(timeWarning, callbackWhenEnd));
    }
    IEnumerator WaitForDisable(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
        gameObject.SetActive(false);

    }
}
