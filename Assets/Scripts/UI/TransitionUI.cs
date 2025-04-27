using DG.Tweening;
using System;
using UnityEngine;

public class TransitionUI : MonoBehaviour
{
    CanvasGroup canvas;
    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        Observer.Instance.Register(EventId.OnTransitionScreen, TransitionUI_OnTransitionScreen);
    }
    private void OnDestroy()
    {
        Observer.Instance.Register(EventId.OnTransitionScreen, TransitionUI_OnTransitionScreen);
        DOTween.Kill(transform);
    }
    public void TransitionUI_OnTransitionScreen(object obj)
    {
        Action action = (Action)obj;
        canvas.alpha = 0;
        canvas.DOFade(1, 0.5f).OnComplete(() =>
        {
            action?.Invoke();
            canvas.DOFade(0f, 1);

        });
    }
}
