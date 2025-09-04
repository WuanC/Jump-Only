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
    //public Dictionary<string, LevelSO> LevelDatas { get; private set; } = new Dictionary<string, LevelSO>();

    public int LevelCount { get; private set; } = 1;
    [Header("Endless Mode")]
    public string ENDLESS_MAP_KEY = "EndlessMap";
    public string ENDLESS_SETTING_KEY = "EndlessSetting";
    public GameObject LevelEndlessPrefabs { get; private set; }
    public EndlessSO EndlessSettings { get; private set; }



    protected override void Awake()
    {
        base.Awake();
        LoadBoost();
        LoadLevelAdventureQuantity();
        //LoadAdventureLevels();
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
    //public void LoadAdventureLevels()
    //{
    //    var handle = Addressables.LoadAssetsAsync<LevelSO>(levelAdventureLabel, (LevelSO level) => {
    //        LevelDatas[level.level] = level;
    //    });
    //    handle.Completed += obj =>
    //    {
    //        if (obj.Status == AsyncOperationStatus.Succeeded)
    //        {
    //            OnLoadAdventureLevelsCompleted?.Invoke();
    //        }
    //        else
    //        {
    //            Debug.Log("Failed to load adventure levels: " + obj.Status);
    //        }
    //    };
    //}
    public void GetAdventureLevel(string name, Action<GameObject> onLoaded)
    {
        var handle = Addressables.LoadAssetAsync<LevelSO>($"Level {name}");
        handle.Completed += obj =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                 GameObject originObj = Instantiate(obj.Result.levelPrefabs);
                 onLoaded?.Invoke(originObj);

            }
            else
            {
                Debug.LogError("Failed to load adventure level: " + obj.Status);
                onLoaded?.Invoke(null);
            }
            Addressables.Release(handle);
        };
    }


    public void LoadLevelAdventureQuantity()
    {
        var handle = Addressables.LoadResourceLocationsAsync(levelAdventureLabel, typeof(LevelSO));
        handle.Completed += obj =>
        {
            LevelCount = obj.Result.Count;
                            OnLoadAdventureLevelsCompleted?.Invoke();
        };
        Addressables.Release(handle); 
    }
    public void LoadEndlessLevel()
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(ENDLESS_MAP_KEY);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                LevelEndlessPrefabs = obj.Result;
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
