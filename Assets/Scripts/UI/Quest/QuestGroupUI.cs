using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGroupUI : MonoBehaviour
{
    [SerializeField] QuestUI questUIPrefab;
    Dictionary<QuestType, Dictionary<int , QuestUI>> quests;
    [SerializeField] Transform dailyQuestParent;
    [SerializeField] Transform achievementParent;

    private void Start()
    {
        QuestManager.Instance.OnUpdateQuest += Instance_OnUpdateQuest;
        foreach(QuestType key in QuestManager.Instance.quests.Keys)
        {
            foreach(int id in QuestManager.Instance.quests[key].Keys)
            {
                if (QuestManager.Instance.quests[key][id].questData.type == QuestType.DailyQuest)
                {
                    QuestUI questUI = Instantiate(questUIPrefab, dailyQuestParent);
                    quests[key][id] = questUI;
                }
                else if(QuestManager.Instance.quests[key][id].questData.type == QuestType.DailyQuest)
                {
                    QuestUI questUI = Instantiate(questUIPrefab, achievementParent);
                    quests[key][id] = questUI;
                }

            }
        }
    }

    private void Instance_OnUpdateQuest(int id, int currentAmount, int targetAmount)
    {
        
    }
    private void OnDestroy()
    {
        QuestManager.Instance.OnUpdateQuest -= Instance_OnUpdateQuest;
    }
}
