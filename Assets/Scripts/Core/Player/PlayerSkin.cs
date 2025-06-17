using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [SerializeField] WorldSkin[] skins;
    private void Awake()
    {
        skins = GetComponentsInChildren<WorldSkin>();
    }
    private void Start()
    {
        EnableSkin();
    }
    public void EnableSkin()
    {
        int idSkin = GameManager.Instance.IdSkinSelected;
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i].skinData.id == idSkin)
            {
                skins[i].gameObject.SetActive(true);
                GetComponent<Player>().SetVisual(skins[i].GetComponent<PlayerVisual>());
            }
            else skins[i].gameObject.SetActive(false);
        }
    }
}
