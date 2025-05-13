using System.Collections;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private const int distanceSpawn = 10;
    private int mapPassCount = 0;
    private int indexCurrentMap = 0;
    public GameObject[] listObstacleInMaps => endlessSettings.data[indexCurrentMap].listObstacleInMap;
    public GameObject[] maps => endlessSettings.data[indexCurrentMap].listMap;

    private EndlessSO endlessSettings;
    [SerializeField] private float timeToUpdateSpeed;
    [field: SerializeField] private float speed;
    Map lastTileMap;
    private float distance = 0;
    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, MapController_OnPlayerDie);
        endlessSettings = Resources.Load<EndlessSO>("LevelEndless/Endess Setting");
        StartCoroutine(UpdateSpeed());
        StartCoroutine(BroadcastSpeed());
        SpawnMap(true);
        SpawnMap();
    }
    private void Update()
    {
        distance += speed * Time.deltaTime;
    }
    public void OnDestroy()
    {

        Observer.Instance.Unregister(EventId.OnPlayerDied, MapController_OnPlayerDie);

    }
    public void MapController_OnPlayerDie(object obj)
    { 
    }
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
            tmpGO.transform.position = Vector3.zero;
        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, maps.Length);
            tmpGO = MyPoolManager.Instance.GetFromPool(maps[UnityEngine.Random.Range(0, maps.Length)], transform);
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
            yield return new WaitForSeconds(timeToUpdateSpeed);
            UpdateSpeed(speed * 1.02f);
        }
    }
    public IEnumerator BroadcastSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Observer.Instance.Broadcast(EventId.OnBroadcastSpeed, distance);
        }
    }

}
