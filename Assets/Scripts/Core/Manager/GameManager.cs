using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] float timeLoadNewScene;

    [Header("Level mode")]
    private const string levelPath = "Levels";
    private Dictionary<string, LevelSO> levelDatas = new();
    public Dictionary<string, LevelSO> Levels => levelDatas;

    private GameObject currentLevelObj;
    [SerializeField] private int currentLevel = 1;
    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            currentLevel = value;
        }
    }


    [Header("Endless mode")]
    private const string endlessPath = "LevelEndless";
    public GameObject levelEndlessPrefabs;

    //Event
    public event Action<string, int> OnLevelChanged;
    public event Action OnClearLevel;

    [Header("Player Spawn Endless")]
    [SerializeField] float radiusCheckObstacle;
    [SerializeField] LayerMask obstacle;
    Player player;
    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }
    private void Start()
    {
        LoadEndlessLevel();
    }
    private void Update()
    {
        
    }
    public void LoadData()
    {
        List<LevelSO> levelSOs = Resources.LoadAll<LevelSO>(levelPath).ToList();
        levelDatas = levelSOs.ToDictionary(level => level.level);

        List<GameObject> list = Resources.LoadAll<GameObject>(endlessPath).ToList();
        levelEndlessPrefabs = list.First();
    }

    public void PlayerWin()
    {
        StartCoroutine(PlayerWinCouroutine());
    }
    public IEnumerator PlayerWinCouroutine()
    {
        Time.timeScale = 0.3f;
        OnClearLevel?.Invoke();
        yield return new WaitForSeconds(timeLoadNewScene);
        Time.timeScale = 1f;

        if (currentLevel == levelDatas.Count)
        {
            Observer.Instance.Broadcast(EventId.OnPlayerCompletedGame, null);
            Destroy(currentLevelObj.gameObject);
            yield break; //do sth
        }
            currentLevel++;
        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() => LoadNewLevel(currentLevel.ToString())));
    }
    public void DeleteCurrentLevel()
    {
        if(currentLevelObj != null)
        {
            Destroy(currentLevelObj.gameObject);
        }
    }
    public void LoadNewLevel(string level = "1")
    { 
        if (currentLevelObj != null) Destroy(currentLevelObj);
        CONSTANT.SaveLevel(level);
        currentLevelObj = Instantiate(levelDatas[level].levelPrefabs);
        OnLevelChanged?.Invoke(level, levelDatas.Count);
    }
    public void LoadEndlessLevel()
    {
        if (currentLevelObj != null) Destroy(currentLevelObj);
        currentLevelObj = Instantiate(levelEndlessPrefabs);
    }

    #region spawn endless

    private Vector2 tmpPos;
    public void RespawnEndless(Player player)
    {
        this.player = player;
        StartCoroutine(SpawnPlayerCheckPoint(radiusCheckObstacle, player));
    }
    IEnumerator SpawnPlayerCheckPoint(float radiusCheck, Player player)
    {
        Vector2 newPos = player.transform.position;
        while (true)
        {
            newPos = (Vector2)Camera.main.transform.position + UnityEngine.Random.insideUnitCircle * radiusCheckObstacle;
            //RaycastHit2D hit = Physics2D.Raycast((Vector2)Camera.main.transform.position, newPos - (Vector2)Camera.main.transform.position, Mathf.Infinity, obstacle);
            Collider2D hit = Physics2D.OverlapPoint(newPos, obstacle);
            if (hit == null)
            {
                break;
            }
            else
            {
                tmpPos = newPos;
                Debug.LogError(hit.gameObject.name);
            }
            yield return null;
        }
        player.transform.position = newPos;
        player.RespawnEndless();
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(Camera.main.transform.position, new Vector3(tmpPos.x, tmpPos.y, 0) - Camera.main.transform.position);
        Gizmos.DrawWireSphere((Vector2)Camera.main.transform.position, radiusCheckObstacle);
    }
    #endregion

}
