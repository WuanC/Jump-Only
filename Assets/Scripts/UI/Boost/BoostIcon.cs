using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostIcon : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image container;

    public void Initial(Sprite icon)
    {
        this.icon.sprite = icon;
    }
    public void UpdateUsePercent(float percent)
    {
        container.fillAmount = percent;
    }
}
