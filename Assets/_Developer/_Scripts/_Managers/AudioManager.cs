using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip _ballCollideSound;
    [SerializeField] private AudioClip _brickBreakSound;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _ballAudioSource;
    [SerializeField] private AudioSource _brickAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void OnEnable()
    {
        GameManager.Instance.OnSoundSwitchChanged += GameManager_OnSoundSwitchChanged;
    }



    private void OnDisable()
    {
        GameManager.Instance.OnSoundSwitchChanged -= GameManager_OnSoundSwitchChanged;
    }

    private void Start()
    {
        PlayMusic();
    }

    private void GameManager_OnSoundSwitchChanged()
    {
        PlayMusic();
    }


    private void PlayMusic() 
    {
        if (GameManager.Instance.GetSoundSwitch() == SoundSwitch.On)
        {
            _musicAudioSource.clip = _backgroundMusic;
            _musicAudioSource.Play();
        }
        else
        {
            StopMusic();
        }
    }

    private void StopMusic()
    {
        _musicAudioSource.Stop();
    }

    public void PlayBallSound()
    {
        if(GameManager.Instance.GetSoundSwitch() == SoundSwitch.On)
        {
            _ballAudioSource.clip = _ballCollideSound;
            _ballAudioSource.Play();
        }
    }

    public void PlayBrickBreakSound()
    {
        if (GameManager.Instance.GetSoundSwitch() == SoundSwitch.On)
        {
            _brickAudioSource.clip = _brickBreakSound;
            _brickAudioSource.Play();
        }
    }



}
