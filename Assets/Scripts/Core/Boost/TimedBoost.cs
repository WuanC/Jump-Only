using System.Collections;
using UnityEngine;

public abstract class TimedBoost : BoostBase, IBoost
{
    [SerializeField] protected float duration;
    [SerializeField] protected float timeLeft;
    public float TimeLeft => timeLeft;

    public bool IsActived { get; protected set; }

    public override void Active()
    {
        if (playerBoost == null) return;
        if (!playerBoost.CanAddBoost(BoostType.Time, this)) return;

        IsActived = true;
        timeLeft = duration;
        Excute();
        StartCoroutine(StartBroadCast());
    }
    public virtual IEnumerator StartBroadCast()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Observer.Instance.Broadcast(EventId.OnUpdateBoost, (boostData.name, timeLeft / duration));
        }

    }
    public override void ResetBoost()
    {
        timeLeft = duration;
    }
    public override void Deactive()
    {
        base.Deactive();
        IsActived = false;
    }
}
