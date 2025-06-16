using UnityEngine;
using UnityEngine.UI;

public class PanelGameMode : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Button shopBtn;
    [SerializeField] Button adventureBtn;
    [SerializeField] Button endlessBtn;

    RectTransform shopRect;
    RectTransform adventureRect;
    RectTransform endlessRect;

    Image shopImg;
    Image adventureImg;
    Image endlessImg;

    float unselectHeight = 220;
    float selectHeight = 250;

    [SerializeField] Sprite unselectSprite;
    [SerializeField] Sprite selectSprite;


    [SerializeField] private PanelHorizontalProcess horizontalProcess;
    private void Awake()
    {
        shopRect = shopBtn.GetComponent<RectTransform>();
        adventureRect = adventureBtn.GetComponent<RectTransform>();
        endlessRect = endlessBtn.GetComponent<RectTransform>();

        shopImg = shopBtn.GetComponent <Image>();
        adventureImg = adventureBtn.GetComponent<Image>();
        endlessImg = endlessBtn.GetComponent<Image>();
    }
    private void Start()
    {

        shopBtn.onClick.AddListener(() =>
        {
            SelectButton(shopRect, shopImg);
            horizontalProcess.ChangePanel(EGamePanel.Shop);
        });
        adventureBtn.onClick.AddListener(() =>
        {
            SelectButton(adventureRect, adventureImg);
            horizontalProcess.ChangePanel(EGamePanel.Adventure);
        });
        endlessBtn.onClick.AddListener(() =>
        {
            SelectButton(endlessRect, endlessImg);
            horizontalProcess.ChangePanel(EGamePanel.Endless);
        });
        horizontalProcess.OnChangedPanel += HorizontalProcess_OnChangedPanel;

        SelectButton(endlessRect, endlessImg);
        horizontalProcess.ChangePanel(EGamePanel.Endless);
    }

    private void HorizontalProcess_OnChangedPanel(EGamePanel obj)
    {
        if(obj == EGamePanel.Endless)
        {
            SelectButton(endlessRect, endlessImg);
        }
        else if(obj == EGamePanel.Shop)
        {
            SelectButton(shopRect, shopImg);
        }
        else if(obj == EGamePanel.Adventure)
        {
            SelectButton(adventureRect, adventureImg);
        }
    }

    private void OnDestroy()
    {
        shopBtn.onClick.RemoveAllListeners();
        adventureBtn.onClick.RemoveAllListeners();
        endlessBtn.onClick.RemoveAllListeners();
    }
    public void ResetScaleButton()
    {
        shopRect.sizeDelta = new Vector2(0, unselectHeight);
        adventureRect.sizeDelta = new Vector2(0, unselectHeight);
        endlessRect.sizeDelta = new Vector2(0, unselectHeight);

        shopImg.sprite = unselectSprite;
        adventureImg.sprite = unselectSprite;
        endlessImg.sprite = unselectSprite;
    }
    public void SelectButton(RectTransform btnRect, Image img)
    {
        ResetScaleButton();
        btnRect.sizeDelta = new Vector2(0, selectHeight);
        img.sprite = selectSprite;

    }
}
