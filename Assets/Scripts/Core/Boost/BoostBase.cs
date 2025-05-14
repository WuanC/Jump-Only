using UnityEngine;

public abstract class BoostBase : MonoBehaviour, IBoost
{
    public BoostSO boostData;
    public  PlayerBoost playerBoost;
    public abstract void Active();
    public abstract void ResetBoost();
    public virtual void Deactive()
    {
        Observer.Instance.Broadcast(EventId.OnRemoveBoost, this);
    }
    public abstract void Excute();
}
