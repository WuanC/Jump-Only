using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSkinUI : MonoBehaviour
{
    public Button leftBtn;
    public Button rightBtn;
    public Button selectBtn;
    public Button unlockBtn;
    public Image visualSkin;
    [SerializeField] StoreSkin[] arrSkins;
    private int indexSelected;
    public void Start()
    {
        indexSelected = 0;
        leftBtn.onClick.AddListener(() => indexSelected--);
        rightBtn.onClick.AddListener(() => indexSelected++);
        
    }
    public void UpdateUI()
    {
        visualSkin.sprite = arrSkins[indexSelected].skinData.icon;
        if (arrSkins[indexSelected].Status)
        {
            //Unlock
            unlockBtn.gameObject.SetActive(false);
            selectBtn.gameObject.SetActive(true);
        }
        else
        {
            unlockBtn.gameObject.SetActive(true);
            selectBtn.gameObject.SetActive(false);
        }
    }
}
