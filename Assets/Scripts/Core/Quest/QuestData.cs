using UnityEngine;

[CreateAssetMenu(menuName = "New Quest", fileName = "SO/Quest")]
public class QuestData : MonoBehaviour
{
    public int id;
    public string questName;
    public Sprite icon;
    public int targetAmount;
    public QuestType type;
}
