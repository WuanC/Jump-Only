using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private const int distanceSpawn = 7;
    private int mapPassCount = 0;
    private int indexCurrentMap = 0;
    public GameObject[] listObstacleInMaps => endlessSettings.data[indexCurrentMap].listObstacleInMap;
    public GameObject[] maps => endlessSettings.data[indexCurrentMap].listMap;

    private EndlessSO endlessSettings;
    [SerializeField] private float timeToUpdateSpeed;
    [SerializeField] private float speed;
    private float tempSpeed;
    private bool isPlayerAlive;
    Map lastTileMap;
    private float distance = 0;
    public HashSet<GameObject> keyObject = new HashSet<GameObject>();

    [Header("Spawn Boost")]
    public GameObject boostWorldPrefab;
    public List<BoostBase> boostBasePrefabs;


    string real = "Endless Setting";
    string test = "TestSetting";
    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, MapController_OnPlayerDie);
        Observer.Instance.Register(EventId.OnPlayerRespawn, MapController_OnPlayerRespawn);
        endlessSettings = Resources.Load<EndlessSO>($"LevelEndless/{real}");
        StartCoroutine(UpdateSpeed());
        StartCoroutine(BroadcastSpeed());
        SpawnMap(true);
        SpawnMap();
        SpawnMap();
        isPlayerAlive = true;
    }
    [SerializeField] LayerMask trapLayer;
    [SerializeField] float distanceDestroy;
    private void Update()
    {
        distance += speed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, distanceDestroy, trapLayer);
            foreach (var item in hit)
            {
                Debug.Log(item.gameObject.name);
                if (item.TryGetComponent<TrapBase>(out TrapBase trap))
                {

                    trap.DestroySelf();
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceDestroy);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceDestroy);
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
        if (isFirst)
        {
            tmpGO = MyPoolManager.Instance.GetFromPool(maps[0], transform);
            keyObject.Add(maps[0]);
            tmpGO.transform.position = Vector3.zero;
        }
        else
        {
            GameObject map = maps[UnityEngine.Random.Range(0, maps.Length)];
            keyObject.Add(map);
            tmpGO = MyPoolManager.Instance.GetFromPool(map, transform);
            tmpGO.transform.position = new Vector3(lastTileMap.transform.position.x, lastTileMap.GetValidPosNextPlace(tmpGO.GetComponent<Map>()), lastTileMap.transform.position.z);
        }

        var tileMap = tmpGO.GetComponent<Map>();
        lastTileMap = tileMap;
        mapPassCount++;
        CheckCanAddNewObjstacle();
        tileMap.Initial(distanceSpawn, speed, this);
    }
    public void CheckCanAddNewObjstacle()
    {
        if (indexCurrentMap == endlessSettings.data.Length - 1) return;
        if (mapPassCount < endlessSettings.data[indexCurrentMap + 1].levelCountPass) return;
        indexCurrentMap++;
    }

    #endregion

    private void UpdateSpeed(float newSpeed)
    {
        if (newSpeed < 0) return;
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
