using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoreSkinGroup : MonoBehaviour
{
    public Image skinBG;
    public Sprite notSkinOwnerBG;

    public TextMeshProUGUI textName;
    public TextMeshProUGUI textCoins;
    public Image coinsIcons;

    public Sprite coinsEnable;
    public Sprite coinsDisable;

    public Button selectBtn;
    public Button unlockBtn;

    public Button watchAdsBtn;

    public Button leftBtn;
    public Button rightBtn;

    const string CHOOSING = "Choosing";
    const string CHOOSE_THIS_SKIN = "Choose this skin";
    const string UNLOCK = "Unlock";



    public TextMeshProUGUI txtStatusSelect;
    public TextMeshProUGUI txtStatusUnlock;
    public Image visualSkin;
    [SerializeField] StoreSkin[] arrSkins;
    private int indexPreview;
    private int idSelectingSkin;
    private void OnEnable()
    {
        indexPreview = 0;

        LoadCharacterUnlock();
        LoadIdCharacterSelected();


        UpdateUI();

    }
    public void Start()
    {

        leftBtn.onClick.AddListener(() => ChangePreviewSkin(false));
        rightBtn.onClick.AddListener(() => ChangePreviewSkin(true));
        selectBtn.onClick.AddListener(() => SelecSkin());
        unlockBtn.onClick.AddListener(UnlockBtnOnClick);


    }
    public void UpdateUI()
    {
        visualSkin.sprite = arrSkins[indexPreview].skinData.icon;
        textCoins.text = arrSkins[indexPreview].Price.ToString();
        textName.text = arrSkins[indexPreview].skinData.skinName;
        if (arrSkins[indexPreview].Owner)
        {
            unlockBtn.gameObject.SetActive(false);
            selectBtn.gameObject.SetActive(true);
            coinsIcons.sprite = coinsDisable;
            //Unlock
            if (idSelectingSkin == arrSkins[indexPreview].skinData.id)
            {
                txtStatusSelect.text = CHOOSING;
                selectBtn.interactable = false;
            }
            else
            {
                txtStatusSelect.text = CHOOSE_THIS_SKIN;
                selectBtn.interactable = true;
            }

        }
        else
        {
            coinsIcons.sprite = coinsEnable;
            if (arrSkins[indexPreview].CanBuy)
            {
                unlockBtn.gameObject.SetActive(true);
                unlockBtn.interactable = true;
                txtStatusUnlock.text = UNLOCK;
            }
            else
            {
                unlockBtn.interactable = false;
                unlockBtn.gameObject.SetActive(true);

                txtStatusUnlock.text = arrSkins[indexPreview].HowToUnlock.ToString();
            }

            
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
        //Validate Coins
        StoreSkin ss = arrSkins[indexPreview];
        Inventory inv = Inventory.Instance;
        if (inv.itemDics[inv.coinsData.Id].quantity >= ss.Price)
        {
            Inventory.Instance.UseItem(new Item(inv.coinsData, -ss.Price));
            UnlockSkin(arrSkins[indexPreview].skinData.id);
        }
        else
        {
            Debug.LogError("Not enough coins");
        }


    }
    public void UnlockSkin(int skinId)
    {
        for (int i = 0; i < arrSkins.Length; i++)
        {
            if (skinId == arrSkins[i].skinData.id)
            {
                arrSkins[i].Owner = true;
                SaveCharacterUnlock();
                UpdateUI();
                return;

            }
        }

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
            if (arrSkins[i].Owner) idUnlocks.Add(arrSkins[i].skinData.id);
        }
        SAVE.SaveUnlockCharacter(idUnlocks);
    }
    public void LoadCharacterUnlock()
    {
        arrSkins[0].Owner = true;
        List<int> idUnlocks = SAVE.LoadUnlockCharacterIds();
        if (idUnlocks == null || idUnlocks.Count == 0) return;
        for(int i = 0; i < idUnlocks.Count; i++)
        {
            for(int j = 0; j < arrSkins.Length; j++)
            {
                if (arrSkins[j].skinData.id == idUnlocks[i])
                {
                    arrSkins[j].Owner = true;
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
                if (arrSkins[i].Owner) indexPreview = i;
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
