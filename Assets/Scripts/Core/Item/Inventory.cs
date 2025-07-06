using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{

    [Header("Coins Item")]
    public ItemDataSO coinsData;

    public event Action<string, Item> OnUpdateItem;
    public Dictionary<string, Item> itemDics = new();

    private void Start()
    {
        LoadData();
    }
    public void LoadData()
    {
        int coinsRemaining = SAVE.GetCoins();
        Item coins = new Item(coinsData, coinsRemaining);
        AddItem(coins);
    }
    public void AddItem(Item item)
    {
        if (itemDics.ContainsKey(item.itemData.Id)){    
            itemDics[item.itemData.Id].quantity += item.quantity;
        }
        else
        {
            itemDics.Add(item.itemData.Id, item);
        }
        if(item.itemData.Id == coinsData.Id)
        {
            Observer.Instance.Broadcast(EventId.OnUpdateCoins, itemDics[item.itemData.Id].quantity );
            SAVE.SaveCoins(itemDics[item.itemData.Id].quantity);
        }


        OnUpdateItem?.Invoke(item.itemData.Id, itemDics[item.itemData.Id]);
    }
    public void UseItem(Item item)
    {
        if(itemDics.ContainsKey(item.itemData.Id))
        {
            itemDics[item.itemData.Id].quantity += item.quantity;
            Item changedItem = itemDics[item.itemData.Id];
            if (itemDics[item.itemData.Id].quantity <= 0 && item.itemData.Id != coinsData.Id)
            {
                itemDics.Remove(item.itemData.Id);
            }
            if(item.itemData.Id == coinsData.Id)
            {
                Observer.Instance.Broadcast(EventId.OnSpendCoins, -item.quantity);
                Observer.Instance.Broadcast(EventId.OnUpdateCoins, changedItem.quantity);
                SAVE.SaveCoins(changedItem.quantity);
            }
            OnUpdateItem?.Invoke(item.itemData.Id, changedItem);
        }

    }
}
