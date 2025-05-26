using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    private QuestData data;
    public Image questIcon;
    public TextMeshProUGUI questTitle;

    public Slider slider;
    public TextMeshProUGUI questProgress;

    public Image iconGift;
    public TextMeshProUGUI quantityGift;

    public Button claimBtn;
    public Image iconClaimSuccess;

    public void SetData(QuestData data)
    {
        this.data = data;
        questIcon.sprite = data.icon;
        questTitle.text = data.questName;

        UpdateProgress(0, data.targetAmount);
        claimBtn.interactable = false;
        iconClaimSuccess.gameObject.SetActive(false);

    }
    public void UpdateProgress(int currentAmount, int targetAmount)
    {
        if (data == null) return;
        questProgress.text = $"{currentAmount}/{targetAmount}";
        slider.value = currentAmount / targetAmount;
    }
}
