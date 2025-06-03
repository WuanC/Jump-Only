using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] List<QuestBase> listQuests;


    public Dictionary<int, QuestBase> achievementQuestDict = new();
    public Dictionary<int, QuestBase> dailyQuestDict = new();
    public event Action<QuestType ,int, int, int> OnUpdateQuest;
    public event Action OnInitializedData;

    private void Start()
    {
       RandomDailyQuest();
    }
    void RandomDailyQuest()
    {
        for(int i = 0; i < listQuests.Count; i++)
        {
            if (listQuests[i].questData.type == QuestType.DailyQuest)
            {
                QuestBase q = Instantiate(listQuests[i], transform);
                q.OnTrackingQuest += QuestManager_OnTrackingQuest;
                dailyQuestDict[q.questData.id] = q;
            }
            else if (listQuests[i].questData.type == QuestType.Achievement)
            {
                QuestBase q = Instantiate(listQuests[i], transform);
                q.OnTrackingQuest += QuestManager_OnTrackingQuest;
                achievementQuestDict[q.questData.id] = q;
            }


        }
        OnInitializedData?.Invoke();
    }
    void QuestManager_OnTrackingQuest(QuestType type,int id)
    {
        if(type == QuestType.DailyQuest)
        {
            QuestBase q = dailyQuestDict[id];
            if(q.questData.targetAmount <= q.CurrentAmount)
            {
                q.OnCompletedQuest();
            }
            OnUpdateQuest?.Invoke(type ,id, q.CurrentAmount, q.questData.targetAmount);
        }
        else if(type == QuestType.Achievement) {
            QuestBase q = achievementQuestDict[id];
            if (q.questData.targetAmount <= q.CurrentAmount)
            {
                q.OnCompletedQuest();
            }
            OnUpdateQuest?.Invoke(type, id, q.CurrentAmount, q.questData.targetAmount);
        }
    }

}
public enum QuestType
{
    DailyQuest,
    Achievement,
}
