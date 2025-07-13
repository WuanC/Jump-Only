using UnityEngine;

public class QuestRevive : QuestBase
{
    public QuestRevive(QuestData data, Gift gift, int currentAmount, bool isClaimed) : base(data, gift, currentAmount, isClaimed)
    {

    }

    public override void Initial()
    {
        Observer.Instance.Register(EventId.OnPlayerRespawn, QuestRevive_OnPlayerRespawn);
    }
    public void QuestRevive_OnPlayerRespawn(object obj)
    {
        EGameMode gameMode = (EGameMode)obj;
        if (EGameMode.Endless != gameMode) return;
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
