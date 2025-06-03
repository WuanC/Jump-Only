using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    private QuestData data;
    private Gift giftData;

    public Image questIcon;
    public TextMeshProUGUI questTitle;

    public Slider slider;
    public TextMeshProUGUI questProgress;

    public Image iconGift;
    public TextMeshProUGUI quantityGift;

    public Button claimBtn;
    public Image notifyOfBtn;

    public Image iconClaimSuccess;

    public CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetData(QuestData data, Gift giftdata)
    {
        this.data = data;
        this.giftData = giftdata;

        questIcon.sprite = data.icon;
        questTitle.text = data.questName;

        UpdateProgress(0, data.targetAmount);

        if (giftdata != null)
        {
            iconGift.sprite = giftdata.GiftWrapper[0].data.Icon;
            quantityGift.text = giftdata.GiftWrapper[0].quantity.ToString();
        }
        else
        {
            iconGift.gameObject.SetActive(false);
            quantityGift.gameObject.SetActive(false);
        }


        claimBtn.interactable = false;
        notifyOfBtn.gameObject.SetActive(false);
        iconClaimSuccess.gameObject.SetActive(false);

        claimBtn.onClick.AddListener(() =>
        {
            claimBtn.gameObject.SetActive(false);
            iconClaimSuccess.gameObject.SetActive(true);
            canvasGroup.alpha = 0.7f;
            if ((giftData != null && giftData.GiftWrapper != null))
            {
                giftdata.CollectGift();
            }

        });

    }
    public void UpdateProgress(int currentAmount, int targetAmount)
    {
        if (data == null) return;
        questProgress.text = $"{currentAmount}/{targetAmount}";
        slider.value = (float)currentAmount / targetAmount;
        if (currentAmount >= targetAmount) QuestCompleted();
    }
    public void QuestCompleted()
    {
        claimBtn.interactable = true;
        notifyOfBtn.gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        claimBtn.onClick.RemoveAllListeners();
    }
}
