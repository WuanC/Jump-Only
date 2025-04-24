using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camPlayer;
    [SerializeField] CinemachineVirtualCamera camDestination;

    private void Start()
    {
        GameManager.Instance.OnClearLevel += GameManager_OnClearLevel;
    }

    private void GameManager_OnClearLevel()
    {
        camPlayer.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnClearLevel -= GameManager_OnClearLevel;
    }
}
