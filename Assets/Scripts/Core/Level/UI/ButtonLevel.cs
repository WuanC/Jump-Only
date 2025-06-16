using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ButtonLevel : MonoBehaviour
{
    public Button btn;
    public TextMeshProUGUI txtLevel;
    private AdventureMenu menu;
    private int levelTarget;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    private void Start()
    {
        btn.onClick.AddListener(() => EnterLevel(levelTarget));
    }
    public void Setup(int levelTarget, AdventureMenu menu)
    {
        this.menu = menu;
        this.levelTarget = levelTarget;
        txtLevel.text = levelTarget.ToString();
    }
    public void EnterLevel(int level)
    {
        GameManager.Instance.CurrentLevel = level;
        menu.SetText(level);
    }
    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }


}

