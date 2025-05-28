using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] List<QuestBase> dailyQuest;
    [SerializeField] List<QuestBase> achievementQuest;

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
        for(int i = 0; i < dailyQuest.Count; i++)
        {
            QuestBase q = Instantiate(dailyQuest[i], transform);
            q.OnTrackingQuest += QuestManager_OnTrackingQuest;
            dailyQuestDict[q.questData.id] = q;
        }
        for(int i = 0; i < achievementQuest.Count; i++)
        {
            QuestBase q = Instantiate(achievementQuest[i], transform);
            q.OnTrackingQuest += QuestManager_OnTrackingQuest;
            achievementQuestDict[q.questData.id] = q;
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
            
        }
    }

}
public enum QuestType
{
    DailyQuest,
    Achievement,
}
