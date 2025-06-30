using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public TextMeshProUGUI txtShield;
    public Image icon;
    CanvasGroup cv;
    public int quantity = 0;
    Tween flick;

    private void Awake()
    {
        cv = GetComponent<CanvasGroup>();
    }

    public void Upadate(int quantity, Sprite itemIcon)
    {
        this.quantity = quantity;
        txtShield.text = this.quantity.ToString();
        icon.sprite = itemIcon;
    }
    public void Flicker()
    {
        if (flick != null && flick.IsActive()) flick.Kill();
        gameObject.SetActive(true);
        flick = cv.DOFade(0.5f, 0.25f)
                 .SetLoops(4, LoopType.Yoyo)
                 .OnComplete(() =>
                 {
                     gameObject.SetActive(false);
                 });
    }


}
