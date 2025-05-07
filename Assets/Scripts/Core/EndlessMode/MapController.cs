using System.Collections;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private const int posDisable = -25;
    private const int posSpawnMap = -5;
    [SerializeField] private float timeToUpdateSpeed;
    [SerializeField] private float speed;
    [SerializeField] GameObject[] maps;
    Map lastTileMap;
    private void Start()
    {
        StartCoroutine(UpdateSpeed());
        SpawnMap(true);
        SpawnMap();
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
            tmpGO = MyPoolManager.Instance.GetFromPool(maps[1], transform);
            tmpGO.transform.position = new Vector3(lastTileMap.transform.position.x, lastTileMap.GetValidPosNextPlace(tmpGO.GetComponent<Map>()), lastTileMap.transform.position.z);
        }

        var tileMap = tmpGO.GetComponent<Map>();
        lastTileMap = tileMap;
        tileMap.Initial(posDisable, posSpawnMap, speed, UpdateMap);
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

}
