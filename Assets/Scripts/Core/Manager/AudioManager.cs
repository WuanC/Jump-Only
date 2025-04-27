using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip collide;
    [SerializeField] AudioClip boom;
    [SerializeField] AudioClip victory;
    bool muteAudio;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
    }
    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, AudioSource_OnPlayerDied);
        Observer.Instance.Register(EventId.OnPlayerColliding, AudioSource_OnPlayerColliding);
        Observer.Instance.Register(EventId.OnPlayerJump, AudioSource_OnPlayerJump);
        Observer.Instance.Register(EventId.OnPlayerWin, AudioSource_OnPlayerWin);
        Observer.Instance.Register(EventId.OnMuteAudio, AudioSource_OnMuteAudio);
        audioSource.volume = CONSTANT.GetCurrentVolume();
        if (audioSource.volume > 0) muteAudio = false;
        else muteAudio = true;
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerDied, AudioSource_OnPlayerDied);
        Observer.Instance.Unregister(EventId.OnPlayerColliding, AudioSource_OnPlayerColliding);
        Observer.Instance.Unregister(EventId.OnPlayerJump, AudioSource_OnPlayerJump);
        Observer.Instance.Register(EventId.OnPlayerJump, AudioSource_OnPlayerWin);
        Observer.Instance.Unregister(EventId.OnMuteAudio, AudioSource_OnMuteAudio);
    }

    public void AudioSource_OnMuteAudio(object obj)
    {
        muteAudio = !muteAudio;
        if (muteAudio)
        {
            audioSource.volume = 0f;

        }
        else
        {
            audioSource.volume = 0.5f;
        }
        CONSTANT.SaveAudio(audioSource.volume);
    }

    void AudioSource_OnPlayerDied(object obj)
    {
        audioSource.PlayOneShot(boom, 1.5f);
    }
    void AudioSource_OnPlayerColliding(object obj)
    {
        audioSource.PlayOneShot(collide, 3f);
    }
    void AudioSource_OnPlayerJump(object obj)
    {
        audioSource.PlayOneShot(jump, 1.5f);
    }
    void AudioSource_OnPlayerWin(object obj)
    {
        audioSource.PlayOneShot(victory, 1.5f);
    }



}
