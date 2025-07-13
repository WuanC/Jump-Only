using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{

    [Header("Coins Item")]
    public ItemDataSO coinsData;

    public List<ItemDataSO> itemDataList;

    public event Action<string, Item> OnUpdateItem;
    public Dictionary<string, Item> itemDics = new();

    private void Start()
    {
        LoadItem();
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
        if (item.itemData.Id == coinsData.Id)
        {
            Observer.Instance.Broadcast(EventId.OnUpdateCoins, itemDics[item.itemData.Id].quantity);
            SAVE.SaveCoins(itemDics[item.itemData.Id].quantity);
        }
        else SaveItem();

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
            else
            {
                SaveItem();
            }

            OnUpdateItem?.Invoke(item.itemData.Id, changedItem);
        }

    }
    public void LoadItem()
    {
        List<ItemSave> itemSaves = SAVE.LoadItem();
        if(itemSaves != null && itemSaves.Count > 0)
        {
            foreach (var itemSave in itemSaves)
            {
                ItemDataSO itemData = itemDataList.Find(x => x.Id == itemSave.ItemDataId);
                if (itemData != null)
                {
                    Item item = new Item(itemData, itemSave.Quantity);
                    AddItem(item);
                }
            }
        }
    }
    public void SaveItem()
    {
        List<ItemSave> itemSaves = new();
        foreach (var key in itemDics.Keys)
        {
            if(key == coinsData.Id)
            {
                continue;
            }
            Item item = itemDics[key];
            ItemSave itemSave = new ItemSave(item.itemData.Id, item.quantity);
            itemSaves.Add(itemSave);

        }
        SAVE.SaveItem(itemSaves);
    }

}
