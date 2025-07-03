using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public event Action<string, Item> OnUpdateItem;
    public Dictionary<string, Item> itemDics = new();
    public void AddItem(Item item)
    {
        if (itemDics.ContainsKey(item.itemData.Id)){    
            itemDics[item.itemData.Id].quantity += item.quantity;
        }
        else
        {
            itemDics.Add(item.itemData.Id, item);
        }
        OnUpdateItem?.Invoke(item.itemData.Id, itemDics[item.itemData.Id]);
    }
    public void UseItem(Item item)
    {
        if(itemDics.ContainsKey(item.itemData.Id))
        {
            itemDics[item.itemData.Id].quantity += item.quantity;
            Item changedItem = itemDics[item.itemData.Id];
            if (itemDics[item.itemData.Id].quantity <= 0)
            {
                itemDics.Remove(item.itemData.Id);
            }
            OnUpdateItem?.Invoke(item.itemData.Id, changedItem);
        }

    }
}
