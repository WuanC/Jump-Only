using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "SO/Item")]
public class ItemDataSO : ScriptableObject
{
    [Header("Base info")]
    [SerializeField] protected string id = "0";
    [SerializeField] protected new string name = "New Item Name";
    [SerializeField] protected Sprite icon = null;

    public string Name => name;
    public Sprite Icon => icon;
    public string Id => id;
}