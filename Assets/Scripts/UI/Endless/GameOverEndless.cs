using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverEndless : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI highScoreTxt;
    [SerializeField] TextMeshProUGUI curScoreTxt;

    [SerializeField] Button homeBtn;
    [SerializeField] Button againBtn;
    [SerializeField] Button continueBtn;

    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, GameOverEndless_OnPlayerDied);
        homeBtn.onClick.AddListener(() => Observer.Instance.Broadcast(EventId.OnBackToMenu, null)); 

    }
    public void GameOverEndless_OnPlayerDied(object obj)
    {
        gameOverPanel.SetActive(true);
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerDied, GameOverEndless_OnPlayerDied);
    }
}
