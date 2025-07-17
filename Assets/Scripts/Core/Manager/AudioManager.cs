using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    AudioSource audioSource;
    [SerializeField] AudioSource audioSfx;

    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip collide;
    [SerializeField] AudioClip boom;
    [SerializeField] AudioClip victory;
    [SerializeField] AudioClip buttonClicked;
    [SerializeField] AudioClip slideShow;
    [SerializeField] AudioClip itemCollect;
    [SerializeField] AudioClip coinsCollect;
    public bool MuteMusic { get; private set; }
    public bool MuteSound { get; private set; }

    [SerializeField] float sfxVolume;
    [SerializeField] float musicVolume;

    float _sfxVolume;

    public event Action OnAudioChanged;
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        
    }
    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerColliding, AudioSource_OnPlayerColliding);
        LoadAudio();

    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerColliding, AudioSource_OnPlayerColliding);
    }
    public void LoadAudio()
    {
        float _musicVolume = SAVE.GetCurrentMusicVolume();
        if (_musicVolume == -1) audioSource.volume = musicVolume;
        else
        {
            audioSource.volume = _musicVolume;
        }
        if (audioSource.volume > 0) MuteMusic = false;
        else MuteMusic = true;
        //
        float soundVolume = SAVE.GetCurrentSoundVolume();
        if(soundVolume == -1) _sfxVolume = sfxVolume;
        else
        {
            _sfxVolume = soundVolume;
        }
        if (_sfxVolume > 0) MuteSound = false;
        else MuteSound = true;

    }
    public void ToggleMusic()
    {
        MuteMusic = !MuteMusic;
        if (MuteMusic)
        {
            audioSource.volume = 0f;

        }
        else
        {
            audioSource.volume = musicVolume;
        }

            OnAudioChanged?.Invoke();
            SAVE.SaveMusic(audioSource.volume);

    }
    public void ToggleSound()
    {
        MuteSound = !MuteSound;
        if(MuteSound)
        {
            _sfxVolume = 0f;
        }
        else
        {
            _sfxVolume = sfxVolume; // Set to your desired sound effect volume
        }
            OnAudioChanged?.Invoke();
            SAVE.SaveSound(_sfxVolume);


    }
    public void ToggleVolumeAll(bool volumeOn)
    {
        if (volumeOn)
        {
            LoadAudio();
        }
        else
        {
            audioSource.volume = 0;
            _sfxVolume = 0;
        }
    }

    public void AudioSource_OnPlayerDied()
    {
        audioSfx.PlayOneShot(boom, _sfxVolume);
    }
    public void AudioSource_OnPlayerColliding(object obj)
    {
        audioSfx.PlayOneShot(collide, _sfxVolume);
    }
    public void AudioSource_OnPlayerJump()
    {
        audioSfx.PlayOneShot(jump, _sfxVolume);
    }
    public void AudioSource_OnPlayerWin()
    {
        audioSfx.PlayOneShot(victory, _sfxVolume);
    }
    public void OnButtonClicked()
    {
        audioSfx.PlayOneShot(buttonClicked, _sfxVolume);
    }
    public void OnSlideShow()
    {
        audioSfx.PlayOneShot(slideShow, _sfxVolume);
    }
    public void OnCollectCoins()
    {
        audioSfx.PlayOneShot(coinsCollect, _sfxVolume);
    }
    public void OnCollectBoost()
    {
        audioSfx.PlayOneShot(itemCollect, _sfxVolume);
    }

}
