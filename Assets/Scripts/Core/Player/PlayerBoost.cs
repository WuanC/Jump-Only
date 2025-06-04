using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBoost : MonoBehaviour
{
    [Header("Invicibility Shield")]
    bool isInvicibility;
    public bool IsInvicibility => isInvicibility;
    public event Action OnPlayerDied;
    public event Action OnEndSlowMotion;
    public event Action OnPlayerBoostDestroy;

    Dictionary<string, BoostBase> boostDic = new();
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
                Debug.Log("update");
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
        //Update UI
    }
    public bool HasKey(string key)
    {
        if(boostDic.ContainsKey(key))
        {
            boostDic[key].ResetBoost();
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

    public void CollectMagnetCoins()
    {

    }
    #endregion

    private void OnDestroy()
    {
        OnPlayerBoostDestroy?.Invoke();
    }
}
