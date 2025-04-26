using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip collide;
    [SerializeField] AudioClip boom;
    [SerializeField] AudioClip victory;

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
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerDied, AudioSource_OnPlayerDied);
        Observer.Instance.Unregister(EventId.OnPlayerColliding, AudioSource_OnPlayerColliding);
        Observer.Instance.Unregister(EventId.OnPlayerJump, AudioSource_OnPlayerJump);
        Observer.Instance.Register(EventId.OnPlayerJump, AudioSource_OnPlayerWin);
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
