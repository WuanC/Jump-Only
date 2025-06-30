using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform parent;
    public ItemUI itemUIPrefabs;
    public Dictionary<string, ItemUI> dicUIs = new();
    private void Start()
    {
        Inventory.Instance.OnUpdateItem += UpdateUI;
        foreach (var item in Inventory.Instance.itemDics)
        {
            UpdateUI(item.Key, item.Value);
        }
    }
    public void UpdateUI(string id, Item item)
    {
        if(item.quantity > 0)
        if (!dicUIs.ContainsKey(id))
        {
            ItemUI newItem = Instantiate(itemUIPrefabs, parent);
            dicUIs.Add(id, newItem);
            newItem.Upadate(item.quantity, item.itemData.Icon);
        }
        else
        {
            dicUIs[id].Upadate(item.quantity, item.itemData.Icon);
        }

        else if(item.quantity < 0 && dicUIs.ContainsKey(id)) 
        {
            dicUIs[id].Upadate(item.quantity, item.itemData.Icon);
            Debug.Log("destroy");
            Destroy(dicUIs[id].gameObject);
            dicUIs.Remove(id);

        }
    }
    private void OnDestroy()
    {
        Inventory.Instance.OnUpdateItem -= UpdateUI;
    }
}
