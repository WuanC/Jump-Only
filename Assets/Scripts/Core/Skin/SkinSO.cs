using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "SO/Skin")]
public class SkinSO : ScriptableObject
{
    public int id;
    public string skinName;
    public Sprite icon;
    public Sprite BG;
}
