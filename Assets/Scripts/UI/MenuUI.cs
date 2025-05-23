using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Start")]
    public GameObject startPanel;
    public Button startBtn;
    public Button continueBtn;
    public Button endlessBtn;
    [SerializeField] GameObject endlessGO;
    [SerializeField] GameObject adventureGO;

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
        exitBtn.onClick.AddListener(ExitBtnOnClick);
        endlessBtn.onClick.AddListener(EndlessBtnOnClick);
        Observer.Instance.Register(EventId.OnPlayerCompletedGame, MenuUI_OnPlayerCompletedGame);
        Observer.Instance.Register(EventId.OnBackToMenu, MenuUI_OnBackToMenu);
    }
    private void OnDestroy()
    {
        startBtn.onClick.RemoveListener(StartBtnOnClick);
        continueBtn.onClick.RemoveListener(ContinueBtnOnClick);
        restartBtn.onClick.RemoveListener(StartBtnOnClick);
        exitBtn.onClick.RemoveListener(ExitBtnOnClick);
        endlessBtn.onClick.RemoveListener(EndlessBtnOnClick);
        Observer.Instance.Unregister(EventId.OnPlayerCompletedGame, MenuUI_OnPlayerCompletedGame);
        Observer.Instance.Unregister(EventId.OnBackToMenu, MenuUI_OnBackToMenu);
    }
    private void OnEnable()
    {
        adventureGO.SetActive(false);
        endlessGO.SetActive(false);
    }
    void StartBtnOnClick()
    {
        adventureGO.SetActive(true);
        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() =>
        {
            GameManager.Instance.CurrentLevel = 1;
            startPanel.SetActive(false);
            endPanel.SetActive(false);
            GameManager.Instance.LoadNewLevel();
        }));
    }
    void ContinueBtnOnClick()
    {
        adventureGO.SetActive(true);
        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() =>
        {
            startPanel.SetActive(false);
            endPanel.SetActive(false);
            GameManager.Instance.CurrentLevel = SAVE.GetCurrentLevel();
            GameManager.Instance.LoadNewLevel(GameManager.Instance.CurrentLevel.ToString());
        }));
    }
    void EndlessBtnOnClick()
    {
        endlessGO.SetActive(true);
        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() =>
        {
            startPanel.SetActive(false);
            endPanel.SetActive(false);
            GameManager.Instance.LoadEndlessLevel();
        }));
    }
    void MenuUI_OnBackToMenu(object obj)
    {
        adventureGO.SetActive(false);
        endlessGO.SetActive(false);
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
