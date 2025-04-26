using UnityEngine;

public class Proxy : MonoBehaviour
{
    Player player;
    float yMaxPlayer;
    private void Awake()
    {
        player = transform.parent.GetComponentInChildren<Player>();
    }
    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, Proxy_OnPlayerDied);
        transform.position = player.transform.position;
        yMaxPlayer = float.MinValue;
    }
    void Proxy_OnPlayerDied(object obj)
    {
        yMaxPlayer = float.MinValue;

    }
    private void Update()
    {

        if (player.transform.position.y > yMaxPlayer && player.gameObject.activeSelf)
        {
            yMaxPlayer = player.transform.position.y;
            transform.position = new Vector3(player.transform.position.x, yMaxPlayer, 0);
        }


    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerDied, Proxy_OnPlayerDied);
    }

}