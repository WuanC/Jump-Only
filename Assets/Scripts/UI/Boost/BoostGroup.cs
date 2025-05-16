using System;
using System.Collections.Generic;
using UnityEngine;

public class BoostGroup : MonoBehaviour
{
    [SerializeField] BoostIcon iconPrefab;
    [SerializeField] Transform parent;

    Dictionary<string, BoostIcon> boostIcons = new();

    private void Start()
    {
        Observer.Instance.Register(EventId.OnUpdateBoost, BoostGroup_OnUpdateBoost);
        Observer.Instance.Register(EventId.OnAddBoost, BoostGroup_OnAddBoost);
        Observer.Instance.Register(EventId.OnRemoveBoost, BoostGroup_OnRemoveBoost);

        Observer.Instance.Register(EventId.OnTransitionScreen, BoostGroup_OnTransitionScreen);
    }
    void BoostGroup_OnAddBoost(object obj)
    {
        BoostBase boostBase = obj as BoostBase;
        if (!boostIcons.ContainsKey(boostBase.boostData.name))
        {
            BoostIcon newIcon = Instantiate(iconPrefab, parent);
            newIcon.Initial(boostBase.boostData.icon);
            boostIcons[boostBase.boostData.name] = newIcon;
        }
    }
    void BoostGroup_OnUpdateBoost(object obj)
    {
        var tuple = obj as Tuple<string, float>;
        if (tuple == null) Debug.Log("null");
        if (!boostIcons.ContainsKey(tuple.Item1)) return;

        boostIcons[tuple.Item1].UpdateUsePercent(tuple.Item2);
    }
    void BoostGroup_OnRemoveBoost(object obj)
    {
        BoostBase boostBase = obj as BoostBase;
        if (!boostIcons.ContainsKey(boostBase.boostData.name)) return;
        Destroy(boostIcons[boostBase.boostData.name].gameObject);
        boostIcons.Remove(boostBase.boostData.name);
    }
    void BoostGroup_OnTransitionScreen(object obj)
    {
        foreach(var key in boostIcons.Keys)
        {
            Destroy(boostIcons[key].gameObject);
        }
        boostIcons.Clear();
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnUpdateBoost, BoostGroup_OnUpdateBoost);
        Observer.Instance.Unregister(EventId.OnAddBoost, BoostGroup_OnAddBoost);
        Observer.Instance.Unregister(EventId.OnRemoveBoost, BoostGroup_OnRemoveBoost);
        Observer.Instance.Unregister(EventId.OnTransitionScreen, BoostGroup_OnTransitionScreen);
    }
}
