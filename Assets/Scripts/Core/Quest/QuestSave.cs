using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSave
{
    public string QuestDataId { get; private set; }
    public string GiftId { get; private set; }
    public EQuest QuestNameType { get; private set; }
    public int CurrentAmount { get; private set; }
    public bool IsClaimed { get; private set; } 
    public QuestSave(string questDataId, string giftId, EQuest questNameType, int currentAmount, bool isClaimed)
    {
        this.QuestDataId = questDataId;
        this.GiftId = giftId;
        this.QuestNameType = questNameType;
        this.CurrentAmount = currentAmount;
        IsClaimed = isClaimed;
    }

}
