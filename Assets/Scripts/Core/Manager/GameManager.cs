using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<string, LevelSO> levelDatas = new();
    private const string levelPath = "Levels";
    [SerializeField] private int currentLevel = 1;
    private GameObject currentLevelObj;
    [SerializeField] float timeLoadNewScene;

    public Dictionary<string, LevelSO> Levels => levelDatas;
    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            currentLevel = value;
        }
    }

    public event Action<string, int> OnLevelChanged;
    public event Action OnClearLevel;

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }
    private void Start()
    {
        Debug.LogError("Game Manager Sstart");
        //LoadNewLevel(currentLevel.ToString());
    }
    public void LoadData()
    {
        List<LevelSO> levelSOs = Resources.LoadAll<LevelSO>(levelPath).ToList();
        levelDatas = levelSOs.ToDictionary(level => level.level);
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


}
