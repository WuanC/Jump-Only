using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] List<QuestBase> dailyQuest;
    [SerializeField] List<QuestBase> achievementQuest;
    public Dictionary<QuestType, Dictionary<int, QuestBase>> quests = new();
    public event Action<int, int, int> OnUpdateQuest;

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
            quests[QuestType.DailyQuest][q.questData.id] = q;
        }
    }
    void QuestManager_OnTrackingQuest(QuestType type,int id)
    {
        if(type == QuestType.DailyQuest)
        {
            QuestBase q = quests[QuestType.DailyQuest][id];
            if(q.questData.targetAmount <= q.CurrentAmount)
            {
                q.OnCompletedQuest();
            }

            OnUpdateQuest?.Invoke(id, q.CurrentAmount, q.questData.targetAmount);
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
