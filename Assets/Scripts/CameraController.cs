using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camPlayer;
    [SerializeField] CinemachineVirtualCamera camDestination;

    [SerializeField] float shakeDuration;
    [SerializeField] int shakeIterator;
    [SerializeField] float shakeStrength;

    private void Start()
    {
        GameManager.Instance.OnClearLevel += Camera_OnClearLevel;
        Observer.Instance.Register(EventId.OnPlayerDied, Camera_PlayerDied);
        Observer.Instance.Register(EventId.OnSpawnEffect, Camera_OnSpawnEffect);

    }

    public void Camera_PlayerDied(object obj)
    {
        var player = camPlayer.Follow;
        camPlayer.Follow = null;
        transform.DOShakePosition(shakeDuration, shakeStrength, shakeIterator).OnComplete(
            () =>
            {
                camPlayer.Follow = player;
                transform.position = Vector3.zero;
            });
    }
    public void Camera_OnSpawnEffect(object obj)
    {
        var player = camPlayer.Follow;
        camPlayer.Follow = null;
        transform.DOShakePosition(0.5f, 1, 10).OnComplete(
            () =>
            {
                camPlayer.Follow = player;
                transform.position = Vector3.zero;
            });
    }


    private void Camera_OnClearLevel()
    {
        camPlayer.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnClearLevel -= Camera_OnClearLevel;
        Observer.Instance.Unregister(EventId.OnPlayerDied, Camera_PlayerDied);
        Observer.Instance.Unregister(EventId.OnSpawnEffect, Camera_OnSpawnEffect);
        DOTween.Kill(gameObject);
    }
}
