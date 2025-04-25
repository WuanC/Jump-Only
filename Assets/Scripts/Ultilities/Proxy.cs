using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Proxy : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] float yMaxPlayer;
    [SerializeField] float yMinCamera;
    Vector2 startPos;
    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, Proxy_OnPlayerDied);
        startPos = player.transform.position;

        transform.position = player.transform.position;
        yMaxPlayer = float.MinValue;
        yMinCamera = float.MinValue;
    }
    void Proxy_OnPlayerDied(object obj)
    {
        yMaxPlayer = startPos.y;
        yMinCamera = float.MinValue;
        transform.position = startPos;

    }
    private void Update()
    {
        if(player.transform.position.y <= yMinCamera)
        {
            player.Died();
        }
        if (player.transform.position.y > yMaxPlayer && player.gameObject.activeSelf)
        {
            yMaxPlayer = player.transform.position.y;
            yMinCamera = virtualCamera.transform.position.y - virtualCamera.m_Lens.OrthographicSize * 4 / 5 ;
        }

        transform.position = new Vector3(player.transform.position.x, yMaxPlayer, 0);
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerDied, Proxy_OnPlayerDied);
    }

}
