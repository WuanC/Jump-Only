using System;
using System.Collections;
using UnityEngine;

public abstract class TimedBoost : BoostBase
{
    [SerializeField] protected float duration;
    [SerializeField] protected float timeLeft;
    public float TimeLeft => timeLeft;

    public bool IsActived { get; protected set; }

    public override void Active()
    {
        if (playerBoost == null) return;
        if (!playerBoost.CanAddBoost(this)) return;
            IsActived = true;
            timeLeft = duration;
            Excute();
            StartCoroutine(StartBroadCast());
     

    }
    protected virtual void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft < 0) Deactive();
    }
    public virtual IEnumerator StartBroadCast()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Observer.Instance.Broadcast(EventId.OnUpdateBoost, Tuple.Create(boostData.name, timeLeft / duration));
        }

    }
    public override bool ResetBoost()
    {
        timeLeft = duration;
        return true;
    }
    public override void Deactive()
    {
        base.Deactive();
        IsActived = false;
        playerBoost.RemoveBoost(this);
    }
}
