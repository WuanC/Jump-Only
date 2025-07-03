using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Endess Setting", menuName = "SO/Endless")]
public class EndlessSO : ScriptableObject
{
    public MapData[] data;
}
[Serializable]
public struct MapData
{
    public int levelCountPass;
    public GameObject[] listObstacleInMap;
    public GameObject[] listMap;
}

