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
    [SerializeField] List<GameObject> obstacleInMaps = new();
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
             _obstaclePosition == null || _obstaclePosition.Length == 0 || obstacleInMaps.Count != 0) return;
        int randomPosCount = UnityEngine.Random.Range(1, _obstaclePosition.Length + 1);

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
            //if(tmpObject.TryGetComponent<TrapBase>(out TrapBase trapBase))
            //{
            //    trapBase.Ready();
            //}
            obstacleInMaps.Add(tmpObject);
            visitedPos.RemoveAt(ramdomPosIndex);
        }
    }
    public void ClearDataWhenDisable()
    {
        foreach (var go in obstacleInMaps)
        {
            go.SetActive(false);
        }
        obstacleInMaps.Clear();
    }


    #endregion
}
