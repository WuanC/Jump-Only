using UnityEngine;

public abstract class BoostBase : MonoBehaviour, IBoost
{
    public BoostSO boostData;
    [HideInInspector] public  PlayerBoost playerBoost;
    public abstract void Active();
    public abstract void ResetBoost();
    public virtual void Deactive()
    {
        Observer.Instance.Broadcast(EventId.OnRemoveBoost, this);
    }
    public virtual void Excute()
    {
        if (playerBoost != null)
        {
            playerBoost.OnPlayerBoostDestroy += Deactive;
        }
    }
}
