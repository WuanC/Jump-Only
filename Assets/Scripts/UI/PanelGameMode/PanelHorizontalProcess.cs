using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelHorizontalProcess : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] float duration;
    Coroutine slideCoroutine;
    float startValue;

    public event Action<EGamePanel> OnChangedPanel;
    private void Start()
    {
        Application.targetFrameRate = 100;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        float value = scrollRect.horizontalNormalizedPosition;
        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        float distanceA = value - startValue;
        float distanceCheck = 0.1f;
        if (Mathf.Abs(startValue - 0) < 0.001)
        {
            if (distanceA > distanceCheck) slideCoroutine = StartCoroutine(DoValue(value, 0.5f, duration, EGamePanel.Endless));
            else slideCoroutine = StartCoroutine(DoValue(value, 0, duration, EGamePanel.Shop));

        }
        else if (Mathf.Abs(startValue - 0.5f) < 0.001)
        {
            if (distanceA > distanceCheck) slideCoroutine = StartCoroutine(DoValue(value, 1f, duration, EGamePanel.Adventure));
            else if (distanceA < -distanceCheck) slideCoroutine = StartCoroutine(DoValue(value, 0f, duration, EGamePanel.Shop));
            else slideCoroutine = StartCoroutine(DoValue(value, 0.5f, duration, EGamePanel.Endless));
        }
        else if (Mathf.Abs(startValue - 1f) < 0.001)
        {
            if (distanceA < -distanceCheck) slideCoroutine = StartCoroutine(DoValue(value, 0.5f, duration, EGamePanel.Endless));
            else slideCoroutine = StartCoroutine(DoValue(value, 1f, duration, EGamePanel.Adventure));
        }
    }

    IEnumerator DoValue(float currentValue, float targetValue, float duration, EGamePanel targetPanel)
    {
        float t = 0;
        while (t <= duration)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(currentValue, targetValue, t / duration);
            t += Time.deltaTime;
            yield return null;

        }
        scrollRect.horizontalNormalizedPosition = targetValue;
        OnChangedPanel?.Invoke(targetPanel);
    }
    public void ChangePanel(EGamePanel targetPanel)
    {
        float value = scrollRect.horizontalNormalizedPosition;
        float targetValue = 0.5f;
        if (targetPanel == EGamePanel.Endless)
        {
            targetValue = 0.5f;
        }
        else if (targetPanel == EGamePanel.Shop)
        {
            targetValue = 0f;
        }
        else
        {
            targetValue = 1f;
        }
        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(DoValue(value, targetValue, duration, targetPanel));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startValue = scrollRect.horizontalNormalizedPosition;
    }
}
