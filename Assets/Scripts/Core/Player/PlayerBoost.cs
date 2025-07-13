using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    [Header("Invicibility Shield")]
    bool isInvicibility;
    public bool IsInvicibility => isInvicibility;
    public event Action OnPlayerDied;
    public event Action OnEndSlowMotion;
    public event Action OnPlayerBoostDestroy;

    Dictionary<string, BoostBase> boostDic = new(); //key = name


    public ItemDataSO boostE;
    public ItemDataSO boostQ;
    public void Start()
    {
        Observer.Instance.Register(EventId.OnUserInput, OnUserInput);
    }
    public bool CanAddBoost(BoostBase boost)
    {
        if (!boostDic.ContainsKey(boost.boostData.name))
        {
            boostDic[boost.boostData.name] = boost;

            Observer.Instance.Broadcast(EventId.OnAddBoost, boost);
        }
        else
        {
            boost.ResetBoost();
            Observer.Instance.Broadcast(EventId.OnUpdateBoost, Tuple.Create(boost.boostData.name, 1f));
            return false;
        }
        return true;
    }

    public void RemoveBoost(BoostBase boost)
    {

        if (boostDic.ContainsKey(boost.boostData.name))
        {
            BoostBase tmp = boostDic[boost.boostData.name];
            boostDic.Remove(boost.boostData.name);

            Destroy(tmp.gameObject);
        }
    }
    public bool HasKey(string key)
    {
        if (boostDic.ContainsKey(key))
        {
            if(boostDic[key].ResetBoost())
            Observer.Instance.Broadcast(EventId.OnUpdateBoost, Tuple.Create(boostDic[key].boostData.name, 1f));
            return true;
        }
        return false;
    }
    #region Invicibility Shield
    public void SetInvicibility(bool isInvicibility)
    {
        this.isInvicibility = isInvicibility;
    }
    public void NotifyEventOnPlayerDied()
    {
        OnPlayerDied?.Invoke();
    }
    #endregion

    #region Slow motion
    #endregion

    #region Magnet Coins 
    #endregion

    public void OnUserInput(object obj)
    {
        InputDirection dir = (InputDirection)obj;
        if(dir == InputDirection.E) {
            var boostPrefabs = ItemDatabase.Instance.Get(boostE.Id);
            if (boostPrefabs != null)
            {
 
                if (!HasKey(boostPrefabs.boostData.name))
                {
                    BoostBase boost = Instantiate(boostPrefabs);
                    boost.playerBoost = this;
                    boost.Active();
                }
            }
        }
        else if(dir == InputDirection.Q)
        {
            var boostPrefabs = ItemDatabase.Instance.Get(boostQ.Id);
            if (boostPrefabs != null)
            {
                if (!HasKey(boostPrefabs.boostData.name))
                {
                    BoostBase boost = Instantiate(boostPrefabs);
                    boost.playerBoost = this;
                    boost.Active();
                }
            }
        }
    }

    private void OnDestroy()
    {
        OnPlayerBoostDestroy?.Invoke();
        Observer.Instance.Unregister(EventId.OnUserInput, OnUserInput);
    }
}
