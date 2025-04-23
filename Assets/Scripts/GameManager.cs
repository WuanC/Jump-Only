using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<string, LevelSO> levelDatas = new();
    private const string levelPath = "Levels";
    private int currentLevel = 1;
    private GameObject currentLevelObj;

    public Dictionary<string, LevelSO> Levels => levelDatas;
    public int CurrentLevel => currentLevel;

    public event Action<string, int> OnLevelChanged;

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }
    private void Start()
    {
        LoadNewLevel(currentLevel.ToString());
    }
    public void LoadData()
    {
        List<LevelSO> levelSOs = Resources.LoadAll<LevelSO>(levelPath).ToList();
        levelDatas = levelSOs.ToDictionary(level => level.level);
    }

    public void PlayerWin()
    {
        Time.timeScale = 0.5f;
        //if (currentLevel == levelDatas.Count) return; //do sth
        //currentLevel++;
        //LoadNewLevel(currentLevel.ToString());

    }
    public void LoadNewLevel(string level)
    {
        if (currentLevelObj != null) Destroy(currentLevelObj);
        currentLevelObj = Instantiate(levelDatas[level].levelPrefabs);
        OnLevelChanged?.Invoke(level, levelDatas.Count);
    }


}
