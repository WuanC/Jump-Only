using UnityEngine;

[CreateAssetMenu(menuName = "SO/Quest", fileName = "New Quest")]
public class QuestData : ScriptableObject
{
    public string id;
    public string questName;
    public Sprite icon;
    public int targetAmount;
    public QuestType type;
    public EQuest questNameType;
}
