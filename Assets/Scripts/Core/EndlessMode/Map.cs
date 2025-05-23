using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    Tilemap tilemap;
    float speed;


    private int distanceSpawn;
    [SerializeField] private int distanceDisable;
    private MapController mapController;
    private bool checkCallback;

    [Header("Obstacle")]
    [SerializeField] Transform[] _obstaclePosition;
    List<int> visitedPos = new();
    [SerializeField] List<GameObject> goInMap = new();


    [Header("Boost")]
    [SerializeField] Transform boostPos;
    [SerializeField] float radius;
    bool isReady = false;
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
    private void Start()
    {
        Observer.Instance.Register(EventId.OnUpdateSpeed, Map_OnUpdateSpeed);

    }
    private void OnDisable()
    {
        isReady = false;
        ClearDataWhenDisable();
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnUpdateSpeed, Map_OnUpdateSpeed);

    }
    public void Initial(int distanceSpawn, float speed, MapController mapController)
    {
        this.distanceSpawn = distanceSpawn;
        this.mapController = mapController;
        this.speed = speed;
        checkCallback = false;
        SpawnObstacle();
        if(boostPos != null)
        SpawnBoost(100f, transform, boostPos.position, radius);
        isReady = true;
    }

    public void Map_OnUpdateSpeed(object obj)
    {
        float newSpeed = (float)obj;
        speed = newSpeed;
    }
    private void Update()
    {
        if (!isReady) return;
        transform.position += speed * new Vector3(0, -1, 0) * Time.deltaTime;

        if (Camera.main.transform.position.y - transform.position.y > distanceSpawn && !checkCallback)
        {
            Debug.Log("Spawn");
            checkCallback = true;
            mapController.UpdateMap();
        }
        if (Camera.main.transform.position.y - transform.position.y > distanceDisable)
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

    #region Spawn GameObject
    public void SpawnObstacle()
    {
        if (mapController.listObstacleInMaps == null || mapController.listObstacleInMaps.Length == 0 ||
             _obstaclePosition == null || _obstaclePosition.Length == 0 || goInMap.Count != 0) return;
        int randomPosCount = UnityEngine.Random.Range(0, _obstaclePosition.Length + 1);

        // log 1, 2, 3

        visitedPos = Enumerable.Range(0, _obstaclePosition.Length).ToList();
        // 1 -> 0
        //2 -> 0, 1
        //3 -> 0, 1, 2
        for (int i = 0; i < randomPosCount; i++)
        {
            int ramdomPosIndex = UnityEngine.Random.Range(0, visitedPos.Count);

            int randomObstacle = UnityEngine.Random.Range(0, mapController.listObstacleInMaps.Length);
            GameObject mapPrefab = mapController.listObstacleInMaps[randomObstacle];
            mapController.keyObject.Add(mapPrefab);
            GameObject tmpObject = MyPoolManager.Instance.GetFromPool(mapPrefab, transform);
            tmpObject.transform.position = _obstaclePosition[visitedPos[ramdomPosIndex]].transform.position;
            goInMap.Add(tmpObject);
            visitedPos.RemoveAt(ramdomPosIndex);
        }
    }
    public void SpawnBoost(float rate, Transform mapParent, Vector2 posititon, float radiusCheckSpawn)
    {

        float randomNumber = UnityEngine.Random.Range(0, 100);
        if (randomNumber > rate) return;
        if (mapParent == null) Debug.Log("null dung");
        GameObject boostWorldGO = MyPoolManager.Instance.GetFromPool(mapController.boostWorldPrefab, mapParent);
        mapController.keyObject.Add(mapController.boostWorldPrefab);
        goInMap.Add(boostWorldGO);
        StartCoroutine(GameManager.Instance.SetPosGOInRange(posititon, radiusCheckSpawn, boostWorldGO.transform));

        if (boostWorldGO.TryGetComponent<BoostWorld>(out BoostWorld bw))
        {
            bw.SetData(mapController.boostBasePrefabs[Random.Range(0, mapController.boostBasePrefabs.Count)]);
        }
    }
    public void ClearDataWhenDisable()
    {
        foreach (var go in goInMap)
        {
            go.SetActive(false);
        }
        goInMap.Clear();
    }
    private void OnDrawGizmosSelected()
    {
        if (boostPos == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(boostPos.position, radius);
    }

    #endregion
}
