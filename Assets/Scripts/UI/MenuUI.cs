using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Start")]
    public GameObject startPanel;
    public Button startBtn;
    public Button continueBtn;


    [Header("End")]
    public GameObject endPanel;
    public Button restartBtn;
    public Button exitBtn;

    public void Start()
    {
        startPanel.SetActive(true);
        startBtn.onClick.AddListener(StartBtnOnClick);
        continueBtn.onClick.AddListener(ContinueBtnOnClick);
        restartBtn.onClick.AddListener(StartBtnOnClick);
        exitBtn.onClick.AddListener(ContinueBtnOnClick);
        Observer.Instance.Register(EventId.OnPlayerCompletedGame, MenuUI_OnPlayerCompletedGame);
        Observer.Instance.Register(EventId.OnBackToMenu, MenuUI_OnBackToMenu);
    }
    private void OnDestroy()
    {
        startBtn.onClick.RemoveListener(StartBtnOnClick);
        continueBtn.onClick.RemoveListener(ContinueBtnOnClick);
        restartBtn.onClick.RemoveListener(StartBtnOnClick);
        exitBtn.onClick.RemoveListener(ContinueBtnOnClick);
        Observer.Instance.Unregister(EventId.OnPlayerCompletedGame, MenuUI_OnPlayerCompletedGame);
        Observer.Instance.Unregister(EventId.OnBackToMenu, MenuUI_OnBackToMenu);
    }
    void StartBtnOnClick()
    {
        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() => {
            GameManager.Instance.CurrentLevel = 1;
            startPanel.SetActive(false);
            endPanel.SetActive(false);
            GameManager.Instance.LoadNewLevel(); }));
    }
    void ContinueBtnOnClick()
    {
        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() => {
            startPanel.SetActive(false);
            GameManager.Instance.CurrentLevel = CONSTANT.GetCurrentLevel();
            GameManager.Instance.LoadNewLevel(GameManager.Instance.CurrentLevel.ToString());
        }));
    }
    void MenuUI_OnBackToMenu(object obj)
    {
        EnableStartMenu();
    }
    void MenuUI_OnPlayerCompletedGame(object obj)
    {
        startPanel.SetActive(false);
        endPanel.SetActive(true);
    }
    public void EnableStartMenu()
    {
        endPanel.SetActive(false);
        startPanel.SetActive(true);
    }
    void ExitBtnOnClick()
    {
        Application.Quit();
    }
}
