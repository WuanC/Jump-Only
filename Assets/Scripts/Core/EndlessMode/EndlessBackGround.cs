using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EndlessBackGround : MonoBehaviour
{
    [SerializeField] GameObject[] backgroundArray;
    [SerializeField] float speed;
    private int outSizeCam;
    private Action destroyCallBack;
    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, EndlessBackGround_OnPlayerDied);
        //for (int i = 0; i < backgroundArray.Length; i++)
        //{
            var background = backgroundArray[0];
            var tileMap = background.GetComponent<Tilemap>();
        Debug.Log(tileMap.cellBounds.position + new Vector3(0, tileMap.cellSize.y * tileMap.size.y, 0) );
            
        //}
    }
    public void Initial(int outSizeCam, Action destroyCallBack)
    {
        this.outSizeCam = outSizeCam;
        this.destroyCallBack = destroyCallBack;
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerDied, EndlessBackGround_OnPlayerDied);
    }
    public void EndlessBackGround_OnPlayerDied(object obj)
    {
        speed = 0;
       
    }
    private void Update()
    {
        transform.position += speed * new Vector3(0, -1, 0) * Time.deltaTime;
        
        if(transform.position.y < outSizeCam)
        {
            destroyCallBack?.Invoke();
        }
        //var tileMap = backgroundArray[0].GetComponent<Tilemap>();
        //tileMap.cellBounds.position = new Vector3Int(0, 0, 0);
        //Debug.Log(tileMap.cellBounds.position + new Vector3(0, backgroundArray[0].transform.position.y, 0));
    }
}
