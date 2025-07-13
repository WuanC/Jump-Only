using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSpendCoins : QuestBase
{
    public QuestSpendCoins(QuestData data, Gift gift, int currentAmount, bool isClaimed) : base(data, gift, currentAmount, isClaimed)
    {
    }

    public override void Initial()
    {
        Observer.Instance.Register(EventId.OnSpendCoins, QuestSpendCoins_OnSpendCoins);
    }
    public void QuestSpendCoins_OnSpendCoins(object obj)
    {
        int coinsAdd = (int)obj;
        if (CurrentAmount < questData.targetAmount)
        {
            CurrentAmount += coinsAdd;
            OnTrackingQuest?.Invoke(questData.type, questData.id);
        }
    }
    public override void OnCompletedQuest()
    {
        base.OnCompletedQuest();
        Observer.Instance.Unregister(EventId.OnSpendCoins, QuestSpendCoins_OnSpendCoins);
    }
}
