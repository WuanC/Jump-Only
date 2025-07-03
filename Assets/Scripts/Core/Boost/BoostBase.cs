using UnityEngine;

public abstract class BoostBase : MonoBehaviour, IBoost
{
    public ItemDataSO boostData;
    [HideInInspector] public  PlayerBoost playerBoost;
    public abstract void Active(); //Call when player dont has boost before
    public abstract bool ResetBoost();
    public virtual void Deactive()
    { 
        Observer.Instance.Broadcast(EventId.OnRemoveBoost, this);
    }
    public virtual void Excute() // Call when active
    {
        if (playerBoost != null)
        {
            playerBoost.OnPlayerBoostDestroy += Deactive;
        }
    }
    
}
