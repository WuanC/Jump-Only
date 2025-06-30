using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGamePlay : MonoBehaviour
{
    [SerializeField] ItemDataSO[] lockData;
    List<string> lockIds = new();
    [SerializeField] Transform parent;
    [SerializeField] ItemUI uiPrefabs;
    Dictionary<string, ItemUI> itemUIs = new();
    private void Awake()
    {

    }
    private void Start()
    {
        Debug.Log(Inventory.Instance);
        StartCoroutine(SetInventory());

        for (int i = 0; i < lockData.Length; i++)
        {
            lockIds.Add(lockData[i].Id);
        }
    }
    IEnumerator SetInventory()
    {
        while (true)
        {
            if (Inventory.Instance != null) break;
            yield return null;

        }
        Inventory.Instance.OnUpdateItem += ItemGamePlay_OnUpdateItem;
    }

    private void ItemGamePlay_OnUpdateItem(string id, Item item)
    {
        if (lockIds.Contains(id)) return;

        if (itemUIs.ContainsKey(id))
        {
        }
        else
        {
            ItemUI ui = Instantiate(uiPrefabs, parent);
            itemUIs.Add(id, ui);
        }
        itemUIs[id].Upadate(item.quantity, item.itemData.Icon);
        itemUIs[id].Flicker();

    }
    private void OnDisable()
    {
        foreach(var key in itemUIs.Keys)
        {
            itemUIs[key].gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        Inventory.Instance.OnUpdateItem -= ItemGamePlay_OnUpdateItem;
    }
}
