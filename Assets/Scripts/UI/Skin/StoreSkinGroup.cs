using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSkinGroup : MonoBehaviour
{
    public Button leftBtn;
    public Button rightBtn;
    public Button selectBtn;
    public TextMeshProUGUI txtStatusSelect;
    public Button unlockBtn;
    public Image visualSkin;
    [SerializeField] StoreSkin[] arrSkins;
    private int indexPreview;
    private int idSelectingSkin;
    public void Start()
    {
        indexPreview = 0;
        leftBtn.onClick.AddListener(() => ChangePreviewSkin(false));
        rightBtn.onClick.AddListener(() => ChangePreviewSkin(true));
        selectBtn.onClick.AddListener(() => SelecSkin());
        unlockBtn.onClick.AddListener(UnlockBtnOnClick);


        LoadCharacterUnlock();
        LoadIdCharacterSelected();


        UpdateUI();

    }
    public void UpdateUI()
    {
        visualSkin.sprite = arrSkins[indexPreview].skinData.icon;
        if (arrSkins[indexPreview].Status)
        {
            unlockBtn.gameObject.SetActive(false);
            selectBtn.gameObject.SetActive(true);
            //Unlock
            if (idSelectingSkin == arrSkins[indexPreview].skinData.id)
            {
                txtStatusSelect.text = "Selected";
                selectBtn.interactable = false;
            }
            else
            {
                txtStatusSelect.text = "Select";
                selectBtn.interactable = true;
            }

        }
        else
        {
            unlockBtn.gameObject.SetActive(true);
            selectBtn.gameObject.SetActive(false);
        }
    }
    public void ChangePreviewSkin(bool right = true)
    {
        if (right)
        {
            if (indexPreview + 1 < arrSkins.Length)
            {
                indexPreview++;
            }
            else
            {
                indexPreview = 0;
            }
        }
        else
        {
            if (indexPreview - 1 >= 0)
            {
                indexPreview--;
            }
            else
            {
                indexPreview = arrSkins.Length - 1;
            }
        }
        UpdateUI();
    }
    public void SelecSkin()
    {
        idSelectingSkin = arrSkins[indexPreview].skinData.id;
        GameManager.Instance.IdSkinSelected = idSelectingSkin;
        Observer.Instance.Broadcast(EventId.OnSelectedSkin, idSelectingSkin);
        SAVE.SaveSelectedCharacterId(idSelectingSkin);
        UpdateUI();
    }
    public void UnlockBtnOnClick() {
        arrSkins[indexPreview].Status = true;
        UpdateUI();
        SaveCharacterUnlock();
        UpdateUI();
    }
    private void OnDestroy()
    {
        leftBtn.onClick.RemoveAllListeners();
        rightBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.RemoveAllListeners();
        unlockBtn.onClick.RemoveAllListeners();
    }
    #region Process SaveLoad Character
    public void SaveCharacterUnlock()
    {
        List<int> idUnlocks = new();
        for(int i = 0; i < arrSkins.Length; i++)
        {
            if (arrSkins[i].Status) idUnlocks.Add(i);
        }
        SAVE.SaveUnlockCharacter(idUnlocks);
    }
    public void LoadCharacterUnlock()
    {
        arrSkins[0].Status = true;
        List<int> idUnlocks = SAVE.LoadUnlockCharacterIds();
        if (idUnlocks == null || idUnlocks.Count == 0) return;
        for(int i = 0; i < idUnlocks.Count; i++)
        {
            for(int j = 0; j < arrSkins.Length; j++)
            {
                if (arrSkins[j].skinData.id == idUnlocks[i])
                {
                    arrSkins[j].Status = true;
                    break;
                }
            }
        }
    }
    public void LoadIdCharacterSelected()
    {
        int idSelected = SAVE.GetCharacterSelectedId();
        if (idSelected == -1)
        {
            for(int i = 0; i < arrSkins.Length;i++)
            {
                if (arrSkins[i].Status) indexPreview = i;
            }
        }
        else
        {
            for (int i = 0; i < arrSkins.Length; i++)
            {
                if (arrSkins[i].skinData.id == idSelected)
                {

                    indexPreview = i;
                }
            }
        }
        idSelectingSkin = arrSkins[indexPreview].skinData.id;
        GameManager.Instance.IdSkinSelected = idSelectingSkin;

    }
    #endregion
}
