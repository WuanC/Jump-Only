using UnityEngine;

public class QuestJumpTarget : QuestBase
{
    public QuestJumpTarget(QuestData data, Gift gift, int currentAmount, bool isClaimed) : base(data, gift, currentAmount, isClaimed)
    {

    }

    public override void Initial()
    {
        Observer.Instance.Register(EventId.OnPlayerJump, QuestJumpTarget_OnPlayerJump);
    }
    public void QuestJumpTarget_OnPlayerJump(object obj)
    {
        if (CurrentAmount < questData.targetAmount)
        {
            CurrentAmount++;
            OnTrackingQuest?.Invoke(questData.type, questData.id);
        }

    }

    public override void OnCompletedQuest()
    {
        Observer.Instance.Unregister(EventId.OnPlayerJump, QuestJumpTarget_OnPlayerJump);
    }
}
