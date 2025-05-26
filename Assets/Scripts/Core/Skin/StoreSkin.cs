using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StoreSkin : MonoBehaviour
{
    [field: SerializeField] public bool CanBuy;
    [field: SerializeField] public int Price;

    public SkinSO skinData;
    [field: SerializeField] public bool Status { get; set; }


}
