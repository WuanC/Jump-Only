using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameEndlessUI : MonoBehaviour
{
    float distance;

    public TextMeshProUGUI textDistance;

    [Header("Game Over")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI highScoreTxt;
    [SerializeField] TextMeshProUGUI curScoreTxt;

    [SerializeField] Button homeBtn;
    [SerializeField] Button againBtn;
    [SerializeField] Button continueBtn;

    private void Start()
    {
        Observer.Instance.Register(EventId.OnBroadcastSpeed, GameEndlessUI_OnBroadcastSpeed);
        Observer.Instance.Register(EventId.OnPlayerDied, GameOverEndless_OnPlayerDied);
        homeBtn.onClick.AddListener(() => {
            GameManager.Instance.DeleteCurrentLevel();
            Observer.Instance.Broadcast(EventId.OnBackToMenu, null);
            });
        continueBtn.onClick.AddListener(() => {
            GameManager.Instance.ContinueEndlessMode();
            gameOverPanel.SetActive(false);
        });
        againBtn.onClick.AddListener(() =>
        {
            gameOverPanel.SetActive(false);
            Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() => {
                GameManager.Instance.RestartEndlessMode();

            }));
        });
    }
    public void GameEndlessUI_OnBroadcastSpeed(object obj)
    {
        distance = (float)obj;
        float tmpDistance = Mathf.Round(distance);
        textDistance.text = (tmpDistance).ToString();
    }
    public void GameOverEndless_OnPlayerDied(object obj)
    {
        float tmpDistance = Mathf.Round(distance);
        SAVE.SaveHighScore(tmpDistance);
        curScoreTxt.text = $"YOUR DISTANCE: {tmpDistance}m";
        highScoreTxt.text = $"HIGH SCORE: {SAVE.GetHighScore()}m";

        gameOverPanel.SetActive(true);
    }
    private void OnDisable()
    {
        gameOverPanel.SetActive(false);
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerDied, GameOverEndless_OnPlayerDied);
        Observer.Instance.Unregister(EventId.OnBroadcastSpeed, GameEndlessUI_OnBroadcastSpeed);
        homeBtn?.onClick.RemoveAllListeners();
        againBtn?.onClick.RemoveAllListeners();
        continueBtn?.onClick.RemoveAllListeners();
    }
}
