using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsageBoost : BoostBase, IBoost
{
    [SerializeField] protected int maxUse;
    [SerializeField] protected int useLeft;
    public int MaxUse => maxUse;

    public override void Active()
    {
        if (playerBoost == null) return;
        if (!playerBoost.CanAddBoost(this)) return;
        Excute();
        useLeft = maxUse;
    }
    public virtual void Use()
    {
        if (useLeft <= 0) return;
        useLeft--;
        Observer.Instance.Broadcast(EventId.OnUpdateBoost, Tuple.Create(boostData.name, (float)useLeft/maxUse));
        if(useLeft == 0) Deactive();
    }
    public override void Deactive()
    {
        base.Deactive();
        playerBoost.RemoveBoost( this);
    }
    public override void ResetBoost()
    {
        useLeft = maxUse;
    }

}
