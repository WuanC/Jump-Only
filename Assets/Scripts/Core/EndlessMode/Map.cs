using System.Collections;
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

    [Header("Line")]
    [SerializeField] Transform[] lineTransform;
    [SerializeField] float distanceSpawnCoins;
    [SerializeField] Transform posEndSpawn;
    [SerializeField] GameObject coinsPrefabs;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] Transform mapCenter;


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

        StartCoroutine(InitCoins());
        isReady = true;

    }
    public IEnumerator InitCoins()
    {
        yield return new WaitForSeconds(0.1f);
        if (mapCenter != null)
            SpawnBoost3Line(10f, transform, mapCenter.position, radius);
        yield return new WaitForSeconds(0.1f);
        SpawnCoinsPattern();
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
    #region Obstacle
    public void SpawnObstacle()
    {
        if (mapController.listObstacleInMaps == null || mapController.listObstacleInMaps.Length == 0 ||
             _obstaclePosition == null || _obstaclePosition.Length == 0 || goInMap.Count != 0) return;
        int randomPosCount = UnityEngine.Random.Range(0, _obstaclePosition.Length);

        // log 1, 2, 3

        visitedPos = Enumerable.Range(0, _obstaclePosition.Length).ToList();
        // 1 -> 0
        //2 -> 0, 1
        //3 -> 0, 1, 2
        for (int i = 0; i < randomPosCount; i++)
        {
            int ramdomPosIndex = UnityEngine.Random.Range(0, visitedPos.Count);

            int randomObstacle = UnityEngine.Random.Range(0, mapController.listObstacleInMaps.Length);
            GameObject obstaclePrefab = mapController.listObstacleInMaps[randomObstacle];
            mapController.keyObject.Add(obstaclePrefab);
            GameObject tmpObject = MyPoolManager.Instance.GetFromPool(obstaclePrefab, transform);
            tmpObject.transform.position = _obstaclePosition[visitedPos[ramdomPosIndex]].transform.position;
            if(tmpObject.TryGetComponent<TrapBase>(out TrapBase trapBase)){
                trapBase.OnTrapDisable += OnGameObjectBeDisable;
            }
            goInMap.Add(tmpObject);
            visitedPos.RemoveAt(ramdomPosIndex);
        }
    }
    #endregion 
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
            bw.OnDisable += OnGameObjectBeDisable;
        }
    }
    public void SpawnBoost3Line(float rate, Transform mapParent, Vector2 posititon, float radiusCheckSpawn)
    {
        if (lineTransform == null || lineTransform.Length <= 0)
        {

            return;
        }
        float randomNumber = UnityEngine.Random.Range(0, 100);
        if (randomNumber > rate) return;
        GameObject boostWorldGO = MyPoolManager.Instance.GetFromPool(mapController.boostWorldPrefab, mapParent);
        mapController.keyObject.Add(mapController.boostWorldPrefab);
        goInMap.Add(boostWorldGO);
        StartCoroutine(GameManager.Instance.SetPosGOInRange(posititon, radiusCheckSpawn, boostWorldGO.transform, lineTransform));

        if (boostWorldGO.TryGetComponent<BoostWorld>(out BoostWorld bw))
        {
            bw.SetData(mapController.boostBasePrefabs[Random.Range(0, mapController.boostBasePrefabs.Count)]);
            bw.OnDisable += OnGameObjectBeDisable;
        }
    }

    public void SpawnCoinsPattern()
    {
        if (lineTransform.Length <= 0) return;
        int lineSelectedIndex = UnityEngine.Random.Range(0, lineTransform.Length);
        SpawnCoins(lineSelectedIndex, posEndSpawn.localPosition.y);
    }
    public void SpawnCoins(int lineIndex, float posEndSpawnY)
    {

        Transform line = lineTransform[lineIndex];
        int indexCoins = 0;
        float lastSpawnY = line.localPosition.y;
        while (true)
        {
            if (lastSpawnY + distanceSpawnCoins >= posEndSpawnY)
            {
                return;
            }
            float yPos = line.localPosition.y + distanceSpawnCoins * indexCoins;
            Vector2 posSpawnLocal = new Vector2(line.localPosition.x, yPos);
            Collider2D collider = Physics2D.OverlapCircle(transform.TransformPoint(posSpawnLocal), 1f, obstacleLayer);

            if (collider == null)
            {
                GameObject coins = MyPoolManager.Instance.GetFromPool(coinsPrefabs, this.transform);
                coins.transform.localPosition = posSpawnLocal;
                goInMap.Add(coins);
                mapController.keyObject.Add(coinsPrefabs);
                coins.GetComponent<ItemWorld>().OnBeCollected += OnGameObjectBeDisable;

            }
            else
            {
                if (lineIndex + 1 < lineTransform.Length)
                {
                    line = lineTransform[lineIndex + 1];
                }
                else if (lineIndex - 1 >= 0)
                {
                    line = lineTransform[lineIndex - 1];
                }



            }
            lastSpawnY = posSpawnLocal.y;
            indexCoins++;
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }

    #endregion
    public void OnGameObjectBeDisable(GameObject obj)
    {
        if (goInMap.Contains(obj))
        {
            goInMap.Remove(obj);
        }
    }
}
