using System;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI heartsText;
    public TextMeshProUGUI heartCountTxt;

    private void Awake()
    {
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

        var tuple = obj as Tuple<string, int>;
        string text = tuple.Item1;
        int currentHearts = tuple.Item2;
        heartsText.text = text.ToString();
        heartCountTxt.text = currentHearts.ToString();
    }
    public void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnUpdateCoins, CurrencyUI_OnUpdateCoins);
        Observer.Instance.Unregister(EventId.OnUpdateHearts, CurrencyUI_OnUpdateHearts);
    }

}
