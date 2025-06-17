using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI heartsText;

    private void Start()
    {
        coinsText.text = GameManager.Instance.Coins.ToString();
        heartsText.text = GameManager.Instance.Hearts.ToString();
        Observer.Instance.Register(EventId.OnUpdateCoins, CurrencyUI_OnUpdateCoins);
        Observer.Instance.Register(EventId.OnUpdateHearts, CurrencyUI_OnUpdateHearts);
    }

    public void CurrencyUI_OnUpdateCoins(object obj)
    {
        int currentCoins = (int)obj;
        coinsText.text = currentCoins.ToString();
    }
    public void CurrencyUI_OnUpdateHearts(object obj)
    {
        int currentHearts = (int)obj;
        heartsText.text = currentHearts.ToString();
    }
    public void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnUpdateCoins, CurrencyUI_OnUpdateCoins);
        Observer.Instance.Unregister(EventId.OnUpdateHearts, CurrencyUI_OnUpdateHearts);
    }

}
