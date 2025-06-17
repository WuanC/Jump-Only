using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPickupBoost : QuestBase
{
    public override void Initial()
    {
        Observer.Instance.Register(EventId.OnPickupBoost, QuestPickupBoost_OnPickupBoost);
    }
    public void QuestPickupBoost_OnPickupBoost(object obj)
    {

        if (CurrentAmount < questData.targetAmount)
        {
            CurrentAmount++;
            OnTrackingQuest?.Invoke(questData.type, questData.id);
        }
    }
    public override void OnCompletedQuest()
    {
        base.OnCompletedQuest();
        Observer.Instance.Unregister(EventId.OnPickupBoost, QuestPickupBoost_OnPickupBoost);
    }
}
