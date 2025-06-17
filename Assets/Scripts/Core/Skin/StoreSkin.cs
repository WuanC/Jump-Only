using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StoreSkin : MonoBehaviour
{
    [field: SerializeField] public bool CanBuy;
    [field: SerializeField] public string HowToUnlock;
    [field: SerializeField] public int Price;
    [field: SerializeField] public bool Owner { get; set; }

    public SkinSO skinData;



}
