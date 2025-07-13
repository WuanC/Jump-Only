using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{


    [SerializeField] List<QuestData> questDatas;
    [SerializeField] List<Gift> giftDatas;

    public Dictionary<string, QuestBase> achievementQuestDict = new();
    public Dictionary<string, QuestBase> dailyQuestDict = new();
    public event Action<QuestType ,string, int, int> OnUpdateQuest;
    public event Action OnInitializedData;

    private void Start()
    {
       CheckNewDay();
        
    }
    void RandomDailyQuest()
    {
        List<QuestSave> questSave = SAVE.LoadAchievementQuest();
        bool containAchivementQuest = questSave != null && questSave.Count > 0;
        for (int i = 0; i < questDatas.Count; i++)
        {
            if (questDatas[i].type == QuestType.DailyQuest)
            {
                QuestBase q = FactoryCreateQuestBase(questDatas[i].questNameType, questDatas[i], giftDatas[UnityEngine.Random.Range(0, giftDatas.Count)], false , 0);
                q.OnTrackingQuest += QuestManager_OnTrackingQuest;
                dailyQuestDict[q.questData.id] = q;
            }
            else if (questDatas[i].type == QuestType.Achievement && !containAchivementQuest)
            {
                    QuestBase q = FactoryCreateQuestBase(questDatas[i].questNameType, questDatas[i], giftDatas[UnityEngine.Random.Range(0, giftDatas.Count)], false, 0);
                    q.OnTrackingQuest += QuestManager_OnTrackingQuest;
                    achievementQuestDict[q.questData.id] = q;
            }
        }
        if (containAchivementQuest) LoadQuest(achievementQuestDict, false);
        else SAVE.SaveAchievementQuest(SaveConvert(achievementQuestDict));
        SAVE.SaveDailyQuest(SaveConvert(dailyQuestDict));



    }
    void QuestManager_OnTrackingQuest(QuestType type,string id)
    {
        if(type == QuestType.DailyQuest)
        {
            QuestBase q = dailyQuestDict[id];
            if(q.questData.targetAmount <= q.CurrentAmount)
            {
                q.OnCompletedQuest();
            }
            OnUpdateQuest?.Invoke(type ,id, q.CurrentAmount, q.questData.targetAmount);
            SAVE.SaveDailyQuest(SaveConvert(dailyQuestDict));
        }
        else if(type == QuestType.Achievement) {
            QuestBase q = achievementQuestDict[id];
            if (q.questData.targetAmount <= q.CurrentAmount)
            {
                q.OnCompletedQuest();
            }
            OnUpdateQuest?.Invoke(type, id, q.CurrentAmount, q.questData.targetAmount);
            SAVE.SaveAchievementQuest(SaveConvert(achievementQuestDict));
        }
    }

    void CheckNewDay()
    {
        DateTime nowUtc = DateTime.UtcNow;
        string dateTimeLastStr = SAVE.GetDateLastTime();
        if (string.IsNullOrEmpty(dateTimeLastStr) || nowUtc.Date > DateTime.Parse(dateTimeLastStr).Date)
        {

            RandomDailyQuest();
            OnInitializedData?.Invoke();
        }
        else
        {
            LoadQuest(dailyQuestDict, true);
            LoadQuest(achievementQuestDict, false);
            OnInitializedData?.Invoke();
        }
        SAVE.SaveDateLastTime(nowUtc.ToString());
    }

    public QuestBase FactoryCreateQuestBase(EQuest type, QuestData questData, Gift gift, bool isClaimed, int currentAmount = 0)
    {
        QuestBase quest = null;
        if (type == EQuest.PickupBoost)
        {
            quest = new QuestPickupBoost(questData, gift, currentAmount, isClaimed);
        }
        else if (type == EQuest.SpendCoins)
        {
            quest = new QuestSpendCoins(questData, gift, currentAmount, isClaimed);
        }
        else if (type == EQuest.JumpTarget)
        {
            quest = new QuestJumpTarget(questData, gift, currentAmount, isClaimed);
        }
        else if (type == EQuest.Revive)
        {
            quest = new QuestRevive(questData, gift, currentAmount, isClaimed);
        }
        return quest;
    }
    public void LoadQuest(Dictionary<string, QuestBase> questDict,bool isDailyQuest = true)
    {
        questDict.Clear();
        List<QuestSave> questSaveList = new();
        if(isDailyQuest) questSaveList = SAVE.LoadDailyQuest();
        else questSaveList = SAVE.LoadAchievementQuest();
        for(int i = 0; i < questSaveList.Count; i++)
        {
            QuestData questData = questDatas.Find(q => q.id == questSaveList[i].QuestDataId);
            Gift gift = giftDatas.Find(g => g.GiftId == questSaveList[i].GiftId);
            int currentAmount = questSaveList[i].CurrentAmount;
            bool isClaimed = questSaveList[i].IsClaimed;
            QuestBase quest = FactoryCreateQuestBase(questSaveList[i].QuestNameType, questData, gift, isClaimed, currentAmount);
            if (quest != null)
            {
                questDict[questData.id] = quest;
                questDict[questData.id].OnTrackingQuest += QuestManager_OnTrackingQuest;
            }
        }

    }
    public List<QuestSave> SaveConvert(Dictionary<string, QuestBase> questDict)
    {
        List<QuestSave> questSave = new();
        foreach (var key in questDict.Keys)
        {
            QuestBase q = questDict[key];
            QuestSave save = new QuestSave(q.questData.id, q.gift.GiftId, q.questData.questNameType, q.CurrentAmount, q.isClaimed);
            questSave.Add(save);
        }
        return questSave;
    }
    public void SaveSingleQuest(string id)
    {
        if (dailyQuestDict.ContainsKey(id))
        {
            SAVE.SaveDailyQuest(SaveConvert(dailyQuestDict));
        }
        else if(achievementQuestDict.ContainsKey(id))
        {
            SAVE.SaveAchievementQuest(SaveConvert(achievementQuestDict));
        }
    }
    

}
public enum QuestType
{
    DailyQuest,
    Achievement,
}
