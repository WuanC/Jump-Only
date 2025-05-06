using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    private const int posOutCam = -25;
    GameObject[] maps;

    private void Start()
    {
        GameObject tmp = MyPoolManager.Instance.GetFromPool(maps[0], transform); 
        var tileMap = tmp.GetComponent<EndlessBackGround>();
    }

}
