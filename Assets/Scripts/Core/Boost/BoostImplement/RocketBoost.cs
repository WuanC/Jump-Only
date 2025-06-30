using UnityEngine;

public class RocketBoost : UsageBoost, IActivationBoost
{
    [SerializeField] float rocketSpeed = 10f;
    [SerializeField] Rocket rocketPrefabs;
    public bool HasBoost(string key)
    {
        return Inventory.Instance.itemDics.ContainsKey(key);
    }

    public override void Excute()
    {
        base.Excute();
        if (HasBoost(boostData.Id))
        {
            Rocket rocket = Instantiate(rocketPrefabs, playerBoost.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            rocket.Init(rocketSpeed, playerBoost.transform.up);
            Item item = new Item(boostData, -1);
            Inventory.Instance.UseItem(item);
            Use();
        }
        else
        {
            Deactive();
        }
    }
    public override void Deactive()
    {
        base.Deactive();
    }
}
