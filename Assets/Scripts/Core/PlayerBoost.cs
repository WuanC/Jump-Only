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


    Dictionary<string, TimedBoost> timeBoostDic = new();
    Dictionary<string, UsageBoost> usageBoostDic = new();
    public bool CanAddBoost(BoostType type, BoostBase boost)
    {
        if (type == BoostType.Usage)
        {
            if (!usageBoostDic.ContainsKey(boost.boostData.name))
            {
                usageBoostDic[boost.boostData.name] = (UsageBoost)boost;
                Observer.Instance.Broadcast(EventId.OnAddBoost, boost);
            }
            else
            {
                boost.ResetBoost();
                Observer.Instance.Broadcast(EventId.OnUpdateSpeed, (boost.boostData.name, 1f));
                return false;
            }

        }
        else if (type == BoostType.Time)
        {
            if (!timeBoostDic.ContainsKey(boost.boostData.name))
            {
                timeBoostDic[boost.boostData.name] = (TimedBoost)boost;
                Observer.Instance.Broadcast(EventId.OnAddBoost, boost);
            }
            else
            {
                boost.ResetBoost();
                Observer.Instance.Broadcast(EventId.OnUpdateSpeed, (boost.boostData.name, 1f));
                return false;
            }
        }
        return true;
    }

    public void RemoveBoost(BoostType type, BoostBase boost)
    {
        if (type == BoostType.Usage)
        {
            if (usageBoostDic.ContainsKey(boost.boostData.name))
            {
                BoostBase tmp = timeBoostDic[boost.boostData.name];
                usageBoostDic.Remove(boost.boostData.name);
                Destroy(tmp);
            }
        }
        else if (type == BoostType.Usage)
        {
            if (timeBoostDic.ContainsKey(boost.boostData.name))
            {
                BoostBase tmp = timeBoostDic[boost.boostData.name];
                timeBoostDic.Remove(boost.boostData.name);
                Destroy(tmp);
            }
        }
        //Update UI
    }

    private void Update()
    {
        UpdateTimeScale();
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
    private float timeScaleLeft;
    public void StartSlowMotion(float timeScale)
    {
        timeScaleLeft = timeScale;
    }
    public void UpdateTimeScale()
    {
        timeScaleLeft -= Time.deltaTime;
        if (timeScaleLeft < 0f)
        {
            OnEndSlowMotion?.Invoke();
        }
    }
    #endregion
}
