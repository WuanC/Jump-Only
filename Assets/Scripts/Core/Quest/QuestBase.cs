using System;
using UnityEngine;

public abstract class QuestBase
{
    public QuestData questData;
    public Action<QuestType, string> OnTrackingQuest; //id
    public int CurrentAmount {  get; protected set; }
    public Gift gift;
    public bool isClaimed;
    private QuestData data;

    public QuestBase(QuestData data, Gift gift, int currentAmount, bool isClaimed)
    {
        this.questData = data;
        this.gift = gift;
        this.CurrentAmount = currentAmount;
        this.isClaimed = isClaimed;
        Initial();
    }

    protected QuestBase(QuestData data, Gift gift, int currentAmount)
    {
        this.data = data;
        this.gift = gift;
        CurrentAmount = currentAmount;
    }

    public abstract void Initial();
    public virtual void OnCompletedQuest()
    {
        OnTrackingQuest = null;
    }
}
