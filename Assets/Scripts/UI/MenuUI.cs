using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Start")]
    public GameObject startPanel;
    public Button endlessBtn;
    public Button endless3LineBtn;
    [SerializeField] GameObject endlessGO;
    [SerializeField] GameObject adventureGO;

    [Header("End")]
    public GameObject endPanel;
    public Button exitBtn;

    public void Start()
    {
        startPanel.SetActive(true);
        exitBtn.onClick.AddListener(ExitBtnOnClick);
        endlessBtn.onClick.AddListener(EndlessBtnOnClick);
        endless3LineBtn.onClick.AddListener(Endless3LineBtnOnClick);
        Observer.Instance.Register(EventId.OnPlayerCompletedGame, MenuUI_OnPlayerCompletedGame);
        Observer.Instance.Register(EventId.OnBackToMenu, MenuUI_OnBackToMenu);
    }
    private void OnDestroy()
    {
        exitBtn.onClick.RemoveListener(ExitBtnOnClick);
        endlessBtn.onClick.RemoveListener(EndlessBtnOnClick);
        endless3LineBtn.onClick.RemoveListener(Endless3LineBtnOnClick);
        Observer.Instance.Unregister(EventId.OnPlayerCompletedGame, MenuUI_OnPlayerCompletedGame);
        Observer.Instance.Unregister(EventId.OnBackToMenu, MenuUI_OnBackToMenu);
    }
    private void OnEnable()
    {
        adventureGO.SetActive(false);
        endlessGO.SetActive(false);
    }
    public void StartBtnOnClick(int level)
    {
        adventureGO.SetActive(true);
        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() =>
        {
            GameManager.Instance.CurrentLevel = level;
            startPanel.SetActive(false);
            endPanel.SetActive(false);
            GameManager.Instance.LoadNewLevel(level.ToString());
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
    void Endless3LineBtnOnClick()
    {
        endlessGO.SetActive(true);
        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() =>
        {
            startPanel.SetActive(false);
            endPanel.SetActive(false);
            GameManager.Instance.LoadEndlessLevel(false);
        }));
    }
    void MenuUI_OnBackToMenu(object obj)
    {

        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() =>
        {
            GameManager.Instance.DeleteCurrentLevel();
            adventureGO.SetActive(false);
            endlessGO.SetActive(false);
            EnableStartMenu();
        }));

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
