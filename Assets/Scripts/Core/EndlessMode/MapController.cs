using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private const int distanceSpawn = 7;
    private int mapPassCount;
    private int indexCurrentMap = 0;
    public GameObject[] listObstacleInMaps => endlessSettings.data[indexCurrentMap].listObstacleInMap;
    public GameObject[] maps => endlessSettings.data[indexCurrentMap].listMap;
    [SerializeField] Transform mapParent;
    private EndlessSO endlessSettings;

    [Header("Map Settings")]
    [SerializeField] private EndlessSO zigzagSettings;
    [SerializeField] private EndlessSO threeLineSettings;
    [SerializeField] private EndlessSO startSettings;
    [SerializeField] EMoveMode curMoveMode;
    [SerializeField] EMoveMode targetMoveMode;

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


    string threeLine = "Endless Setting";
    string zigzag = "ZigzagSettings";
    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, MapController_OnPlayerDie);
        Observer.Instance.Register(EventId.OnPlayerRespawn, MapController_OnPlayerRespawn);
        Observer.Instance.Register(EventId.OnChangeMap, MapController_OnChangeMap);
        Observer.Instance.Register(EventId.OnChangePlayerMovement, MapController_OnChangePlayerMovement);
        threeLineSettings = Resources.Load<EndlessSO>($"LevelEndless/{threeLine}");
        zigzagSettings = Resources.Load<EndlessSO>($"LevelEndless/{zigzag}");

        endlessSettings = startSettings;
        curMoveMode = EMoveMode.Start;
        targetMoveMode = EMoveMode.Start;
        mapPassCount = -1;
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
        Observer.Instance.Unregister(EventId.OnChangeMap, MapController_OnChangeMap);
        Observer.Instance.Unregister(EventId.OnChangePlayerMovement, MapController_OnChangePlayerMovement);
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
            targetMoveMode = EMoveMode.ThreeLine;
            ChangeEndlessMode(EMoveMode.ThreeLine);
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
        tileMap.Initial(distanceSpawn, speed, this, targetMoveMode);

    }
    public void CheckCanAddNewObjstacle()
    {
        if (indexCurrentMap == endlessSettings.data.Length - 1) return;
        if (mapPassCount < endlessSettings.data[indexCurrentMap + 1].levelCountPass) return;
        indexCurrentMap++;
    }

    public void ChangeEndlessMode(EMoveMode targetMoveMode)
    {
        if(targetMoveMode == EMoveMode.ThreeLine)
        {
            endlessSettings = threeLineSettings;
            indexCurrentMap = 0;
            mapPassCount = 0;
        }
        else if(targetMoveMode == EMoveMode.Zigzag)
        {
            endlessSettings = zigzagSettings;
            indexCurrentMap = 0;
            mapPassCount = 0;
        }
    }

    public void MapController_OnChangeMap(object obj)
    {
        if(curMoveMode == EMoveMode.ThreeLine && targetMoveMode == curMoveMode)
        {
            ChangeEndlessMode(EMoveMode.Zigzag);
            targetMoveMode = EMoveMode.Zigzag;
        }
        else if (curMoveMode == EMoveMode.Zigzag && targetMoveMode == curMoveMode)
        {
            ChangeEndlessMode(EMoveMode.ThreeLine);
            targetMoveMode = EMoveMode.ThreeLine;
        }

    }
    public void MapController_OnChangePlayerMovement(object obj)
    {
        EMoveMode eMoveMode = (EMoveMode)obj;
        curMoveMode = eMoveMode;
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
