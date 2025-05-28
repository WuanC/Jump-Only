using System.Collections.Generic;
using UnityEngine;

public class QuestGroupUI : MonoBehaviour
{
    [SerializeField] QuestUI questUIPrefab;


    public Dictionary<int, QuestUI> achievementQuestDict = new();
    public Dictionary<int, QuestUI> dailyQuestDict = new();

    [SerializeField] Transform dailyQuestParent;
    [SerializeField] Transform achievementParent;
    
    private void Start()
    {
        QuestManager.Instance.OnInitializedData += Instance_OnInitializedData;
        QuestManager.Instance.OnUpdateQuest += Instance_OnUpdateQuest;
        Instance_OnInitializedData();

    }

    private void Instance_OnInitializedData()
    {
        foreach (var id in QuestManager.Instance.dailyQuestDict.Keys)
        {
            QuestUI questUI = Instantiate(questUIPrefab, dailyQuestParent);
            dailyQuestDict[id] = questUI;
            questUI.SetData(QuestManager.Instance.dailyQuestDict[id].questData);
        }
        foreach (var id in QuestManager.Instance.achievementQuestDict.Keys)
        {
            QuestUI questUI = Instantiate(questUIPrefab, dailyQuestParent);
            dailyQuestDict[id] = questUI;
            questUI.SetData(QuestManager.Instance.achievementQuestDict[id].questData);
        }
    }

    private void Instance_OnUpdateQuest(QuestType type,int id, int currentAmount, int targetAmount)
    {
        if(type == QuestType.DailyQuest)
        {
            if (!dailyQuestDict.ContainsKey(id)) return;
            dailyQuestDict[id].UpdateProgress(currentAmount, targetAmount);
        }
        else if(type == QuestType.Achievement) {
            if (!achievementQuestDict.ContainsKey(id)) return;
            achievementQuestDict[id].UpdateProgress(currentAmount, targetAmount);
        }
    }
    private void OnDestroy()
    {
        QuestManager.Instance.OnUpdateQuest -= Instance_OnUpdateQuest;
        QuestManager.Instance.OnInitializedData -= Instance_OnInitializedData;
    }
}
