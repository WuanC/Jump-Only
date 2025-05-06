using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    Tilemap tilemap;
    float speed;

    private int posSpawnMap;
    private int posDisable;
    private Action destroyCallBack;
    private bool checkCallback;
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
    private void Start()
    {
        Observer.Instance.Register(EventId.OnUpdateSpeed, Map_OnUpdateSpeed);

    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnUpdateSpeed, Map_OnUpdateSpeed);

    }
    public void Initial(int posDisable, int posSpawn, float speed, Action destroyCallBack)
    {
        this.posDisable = posDisable;
        this.posSpawnMap = posSpawn;
        this.destroyCallBack = destroyCallBack;
        this.speed = speed;
        checkCallback = false;
    }

    public void Map_OnUpdateSpeed(object obj)
    {
        float newSpeed = (float)obj;
        speed = newSpeed;
    }
    private void Update()
    {
        transform.position += speed * new Vector3(0, -1, 0) * Time.deltaTime;
        
        if(transform.position.y < posSpawnMap && !checkCallback)
        {
            checkCallback = true;
            destroyCallBack?.Invoke();
        }
        if(transform.position.y < posDisable)
        {
            gameObject.SetActive(false);
        }
    }
    public float GetTopPos()
    {
        return GetBottomPos() + tilemap.cellSize.y * tilemap.size.y;
    }
    public float GetBottomPos()
    {
        return tilemap.cellBounds.position.y + transform.position.y;
    }
    public float GetValidPosNextPlace(Map nextMap)
    {
        return GetTopPos() + (nextMap.transform.position.y - nextMap.GetBottomPos()) - 1f;
    }

}
