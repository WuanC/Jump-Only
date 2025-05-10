using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level X", menuName = "SO/Levels")]
public class LevelSO : ScriptableObject
{
    public string level;
    public GameObject levelPrefabs;
}
