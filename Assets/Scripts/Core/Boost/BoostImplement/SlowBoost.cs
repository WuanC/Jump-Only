using UnityEngine;

public class SlowBoost : TimedBoost, IActivationBoost
{ 
    [SerializeField] float timeScale;
    float tempTimeScale;
    public bool HasBoost(string key)
    {
        return Inventory.Instance.itemDics.ContainsKey(key);
    }

    public override void Excute()
    {
        base.Excute();
        if (HasBoost(boostData.Id)){
            duration = duration * timeScale;
            timeLeft = duration;
            tempTimeScale = Time.timeScale;
            Time.timeScale = timeScale;
            Item item = new Item(boostData, -1);
            Inventory.Instance.UseItem(item);
        }
        else
        {
            Deactive();
        }

    }
    public override bool ResetBoost()
    {

        if(HasBoost(boostData.Id))
        {
           return base.ResetBoost();
        }
        return false;
        
    }
    public override void Deactive()
    {
        base.Deactive();
        Time.timeScale = 1;
    }

    public bool HasBoost()
    {
        return true;
    }


}
