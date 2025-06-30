using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : Singleton<ItemDatabase>
{
    public Dictionary<string, BoostBase> dicBootbases = new();
    string pathBoost = "Boost";

    protected override void Awake()
    {
        base.Awake();
        var listDic = Resources.LoadAll<BoostBase>(pathBoost);
        foreach (var item in listDic)
        {
            dicBootbases[item.boostData.Id] = item;
        }
    }
    public BoostBase Get(string id)
    {
        if (!dicBootbases.ContainsKey(id)) return null;
        return dicBootbases[id];

    }
}
