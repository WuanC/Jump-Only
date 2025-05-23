using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI textLevel;
    public Button menuBtn;
    public Button musicBtn;


    public void Awake()
    {
        GameManager.Instance.OnLevelChanged += GameManager_OnLevelChanged;

        musicBtn.onClick.AddListener(OnMusicBtnClicked);
        menuBtn.onClick.AddListener(OnMenuBtnClicked);
    }
    private void GameManager_OnLevelChanged(string obj, int maxLevel)
    {
        textLevel.text = $"Level <b>{obj}/{maxLevel.ToString()}</b>";
    }
    void OnMusicBtnClicked()
    {
        Observer.Instance.Broadcast(EventId.OnMuteAudio, null);
    }
    void OnMenuBtnClicked()
    {
        GameManager.Instance.DeleteCurrentLevel();
        Observer.Instance.Broadcast(EventId.OnBackToMenu, null);
    }


    public void OnDestroy()
    {
        GameManager.Instance.OnLevelChanged -= GameManager_OnLevelChanged;

    }
}
