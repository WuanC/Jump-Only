using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DatabaseManager : Singleton<DatabaseManager>
{
    [Header("Boosts")]
    public AssetLabelReference boostLabel;

    public Dictionary<string, BoostBase> DicBootbases { get; private set; } = new();

    [Header("Adventure Levels")]
    [SerializeField] AssetLabelReference levelAdventureLabel;
    public event Action OnLoadAdventureLevelsCompleted;
    public Dictionary<string, LevelSO> LevelDatas { get; private set; } = new Dictionary<string, LevelSO>();
    [Header("Endless Mode")]
    public string ENDLESS_MAP_KEY = "EndlessMap";
    public string ENDLESS_SETTING_KEY = "EndlessSetting";
    public GameObject LevelEndlessPrefabs { get; private set; }
    public EndlessSO EndlessSettings { get; private set; }



    protected override void Awake()
    {
        base.Awake();
        LoadBoost();
        LoadAdventureLevels();
        LoadEndlessLevel();
        LoadEndlessSetting();
    }
    public void LoadBoost()
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>(boostLabel.labelString, (GameObject boost) =>
        {
            BoostBase boostBase = boost.GetComponent<BoostBase>();
            DicBootbases[boostBase.boostData.Id] = boostBase;
        });
        handle.Completed += obj =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Load boosts successfully");
            }
            else
            {
                Debug.LogError("Failed to load boosts: " + handle.Status);
            }
        };
    }
    public BoostBase Get(string id)
    {
        if (!DicBootbases.ContainsKey(id)) return null;
        return DicBootbases[id];

    }
    public void LoadAdventureLevels()
    {
        var handle = Addressables.LoadAssetsAsync<LevelSO>(levelAdventureLabel, (LevelSO level) => {
            LevelDatas[level.level] = level;
        });
        handle.Completed += obj =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                OnLoadAdventureLevelsCompleted?.Invoke();
                Debug.Log("Load levels adventure succesfully");
            }
        };
    }
    public void LoadEndlessLevel()
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(ENDLESS_MAP_KEY);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                LevelEndlessPrefabs = obj.Result;
                //Instantiate(obj.Result);
                Debug.Log("Load endless map successfully");
            }
            else
            {
                Debug.LogError("Failed to load endless map: " + obj.Status);
            }
        };
    }
    public void LoadEndlessSetting()
    {
        var handle = Addressables.LoadAssetAsync<EndlessSO>(ENDLESS_SETTING_KEY);
        handle.Completed += obj =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                EndlessSettings = obj.Result;

            }
            else
            {
                Debug.Log("setting fail");
            }
        };
    }

}
