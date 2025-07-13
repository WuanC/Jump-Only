using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameAdventureUI : MonoBehaviour
{
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textHeartRemaining;


    [SerializeField] GameObject winPanel;
    [SerializeField] Button continueBtn;
    [SerializeField] Button homeBtn;

    [SerializeField] GameObject losePanel;
    [SerializeField] Button restartBtn;
    [SerializeField] Button homeLoseBtn;
    public void Awake()
    {
        GameManager.Instance.OnLevelChanged += GameAdventureUI_OnLevelChanged;
        Observer.Instance.Register(EventId.OnPlayerLoseInAdventure, GameAdventureUI_OnPlayerLose);
        Observer.Instance.Register(EventId.OnPlayerWin, GameAdventureUI_OnPlayerWin);
    }
    private void Start()
    {
        continueBtn.onClick.AddListener(() =>
        {
            winPanel.SetActive(false);
            GameManager.Instance.NextLevelAdventure();
        });
        homeBtn.onClick.AddListener(ReturnHome);

        restartBtn.onClick.AddListener(() =>
        {
            losePanel.SetActive(false);
            GameManager.Instance.RestartCurrentLevel();

        });
        homeLoseBtn.onClick.AddListener(ReturnHome);
    }
    private void GameAdventureUI_OnLevelChanged(string obj, int maxLevel)
    {
        textLevel.text = $"Level <b>{obj}/{maxLevel.ToString()}</b>";
    }
    void GameAdventureUI_OnPlayerLose(object obj)
    {
        int count = (int)obj;
        textHeartRemaining.text = count.ToString();
        if (count < 1)
        losePanel.SetActive(true);
    }
    void GameAdventureUI_OnPlayerWin(object obj)
    {
        winPanel.SetActive(true);
    }

    public void OnDestroy()
    {
        GameManager.Instance.OnLevelChanged -= GameAdventureUI_OnLevelChanged;
        homeBtn.onClick.RemoveListener(ReturnHome);
        continueBtn.onClick.RemoveAllListeners();
        restartBtn.onClick.RemoveAllListeners();
        homeLoseBtn.onClick.RemoveAllListeners();

    }
    public void ReturnHome()
    {
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
        Observer.Instance.Broadcast(EventId.OnBackToMenu, null);
    }
}
