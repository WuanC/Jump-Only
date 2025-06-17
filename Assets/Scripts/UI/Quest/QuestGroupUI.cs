using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGroupUI : MonoBehaviour
{
    [SerializeField] QuestUI questUIPrefab;
    [SerializeField] TextMeshProUGUI textTitle;

    public Dictionary<int, QuestUI> achievementQuestDict = new();
    public Dictionary<int, QuestUI> dailyQuestDict = new();

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
        //Instance_OnInitializedData();
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
            questUI.SetData(QuestManager.Instance.dailyQuestDict[id].questData, QuestManager.Instance.dailyQuestDict[id].gift);
        }
        foreach (var id in QuestManager.Instance.achievementQuestDict.Keys)
        {
            QuestUI questUI = Instantiate(questUIPrefab, achievementParent);
            achievementQuestDict[id] = questUI;
            questUI.SetData(QuestManager.Instance.achievementQuestDict[id].questData, QuestManager.Instance.achievementQuestDict[id].gift);
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
        dailyBtn.onClick.RemoveAllListeners();
        achievementBtn.onClick.RemoveAllListeners();
    }
}
