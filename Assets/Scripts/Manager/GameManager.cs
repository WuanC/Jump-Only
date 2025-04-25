using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<string, LevelSO> levelDatas = new();
    private const string levelPath = "Levels";
    private int currentLevel = 1;
    private GameObject currentLevelObj;
    [SerializeField] float timeLoadNewScene;

    public Dictionary<string, LevelSO> Levels => levelDatas;
    public int CurrentLevel => currentLevel;

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
        LoadNewLevel(currentLevel.ToString());
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

        if (currentLevel == levelDatas.Count) yield break; //do sth
        currentLevel++;
        LoadNewLevel(currentLevel.ToString());
    }
    public void LoadNewLevel(string level)
    { 
        if (currentLevelObj != null) Destroy(currentLevelObj);
        currentLevelObj = Instantiate(levelDatas[level].levelPrefabs);
        OnLevelChanged?.Invoke(level, levelDatas.Count);
    }


}
