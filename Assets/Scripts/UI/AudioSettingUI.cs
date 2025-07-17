using UnityEngine;
using UnityEngine.UI;

public class AudioSettingUI : MonoBehaviour
{
    [SerializeField] Sprite soundOn;
    [SerializeField] Sprite soundOff;
    [SerializeField] Sprite musicOn;
    [SerializeField] Sprite musicOff;

    [SerializeField] Image musicImg;
    [SerializeField] Image soundImg;

    [SerializeField] Button musicBtn;
    [SerializeField] Button soundBtn;
    private void Start()
    {
        SetUpImgAudio();
        musicBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.ToggleMusic();
        });
        soundBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.ToggleSound();
        });
        AudioManager.Instance.OnAudioChanged += SetUpImgAudio;

    }

    private void OnDestroy()
    {
        musicBtn.onClick.RemoveAllListeners();
        soundBtn.onClick.RemoveAllListeners();
    }
    public void SetUpImgAudio()
    {
        if (AudioManager.Instance.MuteMusic)
        {
            musicImg.sprite = musicOff;
        }
        else
        {
            musicImg.sprite = musicOn;
        }

        if (AudioManager.Instance.MuteSound)
        {
            soundImg.sprite = soundOff;
        }
        else
        {
            soundImg.sprite = soundOn;
        }
    }
}
