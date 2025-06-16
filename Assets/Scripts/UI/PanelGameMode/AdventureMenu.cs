using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

public class AdventureMenu : MonoBehaviour, IPointerDownHandler
{
    public Button playBtn;
    public TextMeshProUGUI levelTxt;
    int selectedLevel;


    [Header("Spawn Button")]
    [SerializeField] GameObject levelBtn;
    float[] xPosArr = { -400, 0, 400 };
    [SerializeField] float spaceY;
    [SerializeField] RectTransform contentRect;
    [SerializeField] private int totalBtn;
    [SerializeField] private float startYPos;

    List<ButtonLevel> buttons = new();


    public void OnPointerDown(PointerEventData eventData)
    {
        selectedLevel = SAVE.GetUnlockLevel();
        SetText(selectedLevel);
    }

    private void Start()
    {
        selectedLevel = SAVE.GetUnlockLevel();
        SetText(selectedLevel);

        playBtn.onClick.AddListener(OnPlayBtnClicked);
        Observer.Instance.Register(EventId.OnUnlockNewLevel, LevelGenerator_OnUnlockNewLevel);

        totalBtn = GameManager.Instance.Levels.Count;
        int j = 1;
        int indexPlus = 1;
        for (int i = 0; i < totalBtn; i++)
        {
            GameObject btn = Instantiate(levelBtn, contentRect.transform);
            RectTransform rt = btn.GetComponent<RectTransform>();
            ButtonLevel btnLevel = btn.GetComponent<ButtonLevel>();
            buttons.Add(btnLevel);
            btnLevel.Setup(i + 1, this);
            rt.anchoredPosition = new Vector2(xPosArr[j], -i * spaceY + startYPos);
            j += indexPlus;
            if (j >= 2) indexPlus = -1;
            else if (j <= 0) indexPlus = +1;
        }
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalBtn * spaceY - 2 * startYPos);

        UpdateButton();
    }
    private void OnDestroy()
    {
        playBtn.onClick.RemoveAllListeners();
        Observer.Instance.Unregister(EventId.OnUnlockNewLevel, LevelGenerator_OnUnlockNewLevel);
    }
    public void LevelGenerator_OnUnlockNewLevel(object obj)
    {
        UpdateButton();
    }
    public void OnPlayBtnClicked()
    {
        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() =>
        {
            GameManager.Instance.LoadNewLevel(selectedLevel.ToString());
        }));
    }
    public void UpdateButton()
    {
        int levelUnlock = SAVE.GetUnlockLevel();
        for (int i = 0; i < buttons.Count; i++)
        {

            if (i > levelUnlock - 1)
            {
                buttons[i].btn.interactable = false;
            }
            else
            {
                buttons[i].btn.interactable = true;
            }
        }
    }
    public void SetText(int level)
    {
        selectedLevel = level;
        levelTxt.text = level.ToString();

    }
}
