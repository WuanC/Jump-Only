using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI textLevel;

    [Header("Settings")]
    public GameObject settingsPanel;
    public Button settingsBtn;

    public Button closeBtn;
    public Button againBtn;
    public Button menuBtn;


    public void Awake()
    {
        GameManager.Instance.OnLevelChanged += GameManager_OnLevelChanged;

        menuBtn.onClick.AddListener(OnMenuBtnClicked);
        closeBtn.onClick.AddListener(OnCloseBtnClicked);
        againBtn.onClick.AddListener(OnAgainBtnClicked);
        settingsBtn.onClick.AddListener(OnSettingBtnClicked);
    }
    private void GameManager_OnLevelChanged(string obj, int maxLevel)
    {
        textLevel.text = $"Level <b>{obj}/{maxLevel.ToString()}</b>";
    }
    void OnMenuBtnClicked()
    {
        
        Time.timeScale = 1f;
        settingsPanel.gameObject.SetActive(false);
        Observer.Instance.Broadcast(EventId.OnBackToMenu, null);
    }
    void OnCloseBtnClicked()
    {
        Time.timeScale = 1f;
        settingsBtn.gameObject.SetActive(true);
    }
    void OnAgainBtnClicked()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.RestartCurrentLevel();
    }
    void OnSettingBtnClicked()
    {
        Time.timeScale = 0f;
        settingsPanel.gameObject.SetActive(true);
    }
    public void OnDestroy()
    {
        GameManager.Instance.OnLevelChanged -= GameManager_OnLevelChanged;
        menuBtn.onClick.RemoveListener(OnMenuBtnClicked);
        closeBtn.onClick.RemoveListener(OnCloseBtnClicked);
        againBtn.onClick.RemoveListener(OnAgainBtnClicked);
        settingsBtn.onClick.RemoveListener(OnSettingBtnClicked);


    }
}
