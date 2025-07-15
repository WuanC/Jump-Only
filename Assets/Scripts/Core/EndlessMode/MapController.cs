using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private const int distanceSpawn = 7;
    private int mapPassCount;
    private int indexCurrentMap = 0;
    public GameObject[] listObstacleInMaps => DatabaseManager.Instance.EndlessSettings.data[indexCurrentMap].listObstacleInMap;
    public GameObject[] maps => DatabaseManager.Instance.EndlessSettings.data[indexCurrentMap].listMap;
    [SerializeField] Transform mapParent;
   // private EndlessSO endlessSettings;

    [Header("Map Settings")]

    [SerializeField] private float timeToUpdateSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    private float tempSpeed;
    private bool isPlayerAlive;
    Map lastTileMap;
    private float distance = 0;
    public HashSet<GameObject> keyObject = new HashSet<GameObject>();

    [Header("Spawn Boost")]
    public GameObject boostWorldPrefab;
    //public List<BoostBase> boostBasePrefabs;

    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, MapController_OnPlayerDie);
        Observer.Instance.Register(EventId.OnPlayerRespawn, MapController_OnPlayerRespawn);
        mapPassCount = 0;
        StartCoroutine(UpdateSpeed());
        StartCoroutine(BroadcastSpeed());
        SpawnMap(true);
        SpawnMap();
        isPlayerAlive = true;
    }

    private void Update()
    {
        distance += speed * Time.deltaTime;
    }
    public void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerRespawn, MapController_OnPlayerRespawn);
        Observer.Instance.Unregister(EventId.OnPlayerDied, MapController_OnPlayerDie);
        foreach (GameObject obj in keyObject)
        {
            MyPoolManager.Instance.DeleteKey(obj);
        }

    }
    #region Player Health
    public void MapController_OnPlayerDie(object obj)
    {
        isPlayerAlive = false;
        tempSpeed = speed;
        UpdateSpeed(0);
    }

    public void MapController_OnPlayerRespawn(object obj)
    {
        isPlayerAlive = true;
        UpdateSpeed(tempSpeed);
    }

    #endregion

    #region Spawn Map
    public void UpdateMap()
    {
        SpawnMap();
    }
    public void SpawnMap(bool isFirst = false)
    {
        GameObject tmpGO = null;
        mapPassCount++;
        if (isFirst)
        {
            tmpGO = MyPoolManager.Instance.GetFromPool(maps[0], mapParent);
            keyObject.Add(maps[0]);
            tmpGO.transform.position = Vector3.zero;
        }
        else
        {
            GameObject map = maps[UnityEngine.Random.Range(0, maps.Length)];
            keyObject.Add(map);
            tmpGO = MyPoolManager.Instance.GetFromPool(map, mapParent);
            tmpGO.transform.position = new Vector3(lastTileMap.transform.position.x, lastTileMap.GetValidPosNextPlace(tmpGO.GetComponent<Map>()), lastTileMap.transform.position.z);
        }

        var tileMap = tmpGO.GetComponent<Map>();
        lastTileMap = tileMap;


        CheckCanAddNewObjstacle();
        tileMap.Initial(distanceSpawn, speed, this);

    }
    public void CheckCanAddNewObjstacle()
    {
        if (indexCurrentMap == DatabaseManager.Instance.EndlessSettings.data.Length - 1) return;
        if (mapPassCount < DatabaseManager.Instance.EndlessSettings.data[indexCurrentMap + 1].levelCountPass) return;
        indexCurrentMap++;
    }

    #endregion

    private void UpdateSpeed(float newSpeed)
    {
        if (newSpeed < 0 || newSpeed > maxSpeed) return;
        speed = newSpeed;
        Observer.Instance.Broadcast(EventId.OnUpdateSpeed, speed);
    }
    public IEnumerator UpdateSpeed()
    {
        while (true)
        {
            if (!isPlayerAlive)
            {
                yield return null;
                continue;
            }
            yield return new WaitForSeconds(timeToUpdateSpeed);
            UpdateSpeed(speed * 1.1f);
        }
    }
    public IEnumerator BroadcastSpeed()
    {
        while (true)
        {
            if (!isPlayerAlive)
            {
                yield return null;
                continue;
            }
            yield return new WaitForSeconds(0.1f);
            Observer.Instance.Broadcast(EventId.OnBroadcastSpeed, distance);
        }
    }


}
