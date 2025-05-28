using UnityEngine;

[CreateAssetMenu(menuName = "SO/Quest", fileName = "New Quest")]
public class QuestData : ScriptableObject
{
    public int id;
    public string questName;
    public Sprite icon;
    public int targetAmount;
    public QuestType type;
}
