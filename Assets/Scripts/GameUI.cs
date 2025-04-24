using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI textLevel;
    public void Awake()
    {
        GameManager.Instance.OnLevelChanged += GameManager_OnLevelChanged;
    }

    private void GameManager_OnLevelChanged(string obj, int maxLevel)
    {
        Debug.Log("Change UI");
        textLevel.text = $"Level <b>{obj}/{maxLevel.ToString()}</b>";
    }
    public void OnDestroy()
    {
        GameManager.Instance.OnLevelChanged -= GameManager_OnLevelChanged;
    }
}
