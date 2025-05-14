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

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }
    private void Start()
    {
        Debug.LogError("Game Manager Start");
        //LoadEndlessLevel();
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


}
