using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGroupUI : MonoBehaviour
{
    [SerializeField] QuestUI questUIPrefab;
    [SerializeField] TextMeshProUGUI textTitle;

    public Dictionary<string, QuestUI> achievementQuestDict = new();
    public Dictionary<string, QuestUI> dailyQuestDict = new();

    [SerializeField] Transform dailyQuestParent;
    [SerializeField] Transform achievementParent;
    RectTransform daily;
    RectTransform achievement;


    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Button dailyBtn;
    [SerializeField] Button achievementBtn;
    private void Awake()
    {
        daily = dailyQuestParent.GetComponent<RectTransform>();
        achievement = achievementParent.GetComponent<RectTransform>();

    }
    private void OnEnable()
    {
        QuestManager.Instance.OnInitializedData += Instance_OnInitializedData;
    }
    private void Start()
    {

        QuestManager.Instance.OnUpdateQuest += Instance_OnUpdateQuest;
        dailyBtn.onClick.AddListener(() =>
        {
            ActiveDailyPanel();
        });
        achievementBtn.onClick.AddListener(() =>
        {
            ActiveDailyPanel(false);
        });
        ActiveDailyPanel();

    }
    public void ActiveDailyPanel(bool isActive = true)
    {
        if (isActive)
        {
            scrollRect.content = daily;
            textTitle.text = "Daily Quest";
        }
        else
        {
            scrollRect.content = achievement;
            textTitle.text = "Achievement";
        }

        dailyQuestParent.gameObject.SetActive(isActive);
        achievementParent.gameObject.SetActive(!isActive);
        dailyBtn.interactable = !isActive;
        achievementBtn.interactable = isActive;
    }
    private void Instance_OnInitializedData()
    {
        foreach (var id in QuestManager.Instance.dailyQuestDict.Keys)
        {
            QuestUI questUI = Instantiate(questUIPrefab, dailyQuestParent);
            dailyQuestDict[id] = questUI;
            QuestBase q = QuestManager.Instance.dailyQuestDict[id];
            questUI.SetData(q.questData, q.gift, q.CurrentAmount, q.isClaimed, q);
        }
        foreach (var id in QuestManager.Instance.achievementQuestDict.Keys)
        {
            QuestUI questUI = Instantiate(questUIPrefab, achievementParent);
            achievementQuestDict[id] = questUI;
            QuestBase q = QuestManager.Instance.achievementQuestDict[id];
            questUI.SetData(q.questData, q.gift, q.CurrentAmount, q.isClaimed, q);
        }
    }

    private void Instance_OnUpdateQuest(QuestType type,string id, int currentAmount, int targetAmount)
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
        dailyBtn.onClick.RemoveAllListeners();
        achievementBtn.onClick.RemoveAllListeners();
    }
}
