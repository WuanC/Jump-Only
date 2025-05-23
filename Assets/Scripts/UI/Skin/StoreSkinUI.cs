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
        
    }
}
