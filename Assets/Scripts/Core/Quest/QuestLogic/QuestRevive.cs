using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRevive : QuestBase
{
    [SerializeField] EGameMode gameMode;
    public override void Initial()
    {
        Observer.Instance.Register(EventId.OnPlayerRespawn, QuestRevive_OnPlayerRespawn);
    }
    public void QuestRevive_OnPlayerRespawn(object obj)
    {
        EGameMode gameMode = (EGameMode)obj;
        if (this.gameMode != gameMode) return;
        if (CurrentAmount < questData.targetAmount)
        {
            CurrentAmount++;
            OnTrackingQuest?.Invoke(questData.type, questData.id);
        }
    }
    public override void OnCompletedQuest()
    {
        base.OnCompletedQuest();
        Observer.Instance.Unregister(EventId.OnPlayerRespawn, QuestRevive_OnPlayerRespawn);
    }
}
