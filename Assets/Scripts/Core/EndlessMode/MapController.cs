using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private const int posDisable = -25;
    private const int posSpawnMap = -5;
    private int mapPassCount = -2;
    private int indexCurrentMap = 0;
    public GameObject[] listObstacleInMaps => endlessSettings.data[indexCurrentMap].listObstacleInMap;

    private EndlessSO endlessSettings;
    [SerializeField] private float timeToUpdateSpeed;
    [SerializeField] private float speed;
    [SerializeField] GameObject[] maps;
    Map lastTileMap;
    private void Start()
    {
        endlessSettings = Resources.Load<EndlessSO>("LevelEndless/Endess Setting");
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
        mapPassCount++;
        CheckCanAddNewObjstacle();
        tileMap.Initial(posDisable, posSpawnMap, speed, this);
    }
    public void CheckCanAddNewObjstacle()
    {
        if (indexCurrentMap == endlessSettings.data.Length - 1) return;
        if (mapPassCount <= endlessSettings.data[indexCurrentMap + 1].levelCountPass) return;
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

}
