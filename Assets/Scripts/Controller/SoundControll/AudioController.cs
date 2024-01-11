using TigerForge;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    [Header("Main Settings:")]
    [Range(0, 1)]
    public float musicVolume = 1f;
    /// the sound fx volume
    [Range(0, 1)]
    public float sfxVolume = 1f;

    public AudioSource musicAus;
    public AudioSource sfxAus;

    [Header("Game sounds and musics: ")]
    public AudioClip enemyHit;
    public AudioClip shoot;
    public AudioClip playerHit;
    public AudioClip shieldHit;
    public AudioClip getItem;
    public AudioClip shieldBreak;
    public AudioClip upgradeDone;
    public AudioClip warnningBoss;
    public AudioClip bossAngry;
    public AudioClip bossDeath;
    public AudioClip bossStart;
    public AudioClip gameover;
    public AudioClip win;

    public AudioClip click; 

    public AudioClip[] backgroundMusics;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        CheckMusicDefault();
        CheckSoundDefault();
        EventManager.StartListening(EventConstants.UPDATE_VOLUME_MUSIC, UpdateVolumeMusic);
        EventManager.StartListening(EventConstants.UPDATE_VOLUME_SOUND, UpdateVolumeSound);
        CheckUpdateSoundAndMusic();
    }

    private void CheckSoundDefault()
    {
        if(PlayerDataManager.Instance.GetSoundSetting() == -1)
        {
            PlayerDataManager.Instance.SetSoundSetting(1);
        }
    }

    private void CheckMusicDefault()
    {
        if (PlayerDataManager.Instance.GetMusicSetting() == -1)
        {
            PlayerDataManager.Instance.SetMusicSetting(1);
        }
    }

    private void CheckUpdateSoundAndMusic()
    {
        sfxVolume = PlayerDataManager.Instance.GetSoundSetting();
        musicVolume = PlayerDataManager.Instance.GetMusicSetting();
    }

    private void UpdateVolumeSound()
    {
        sfxVolume = PlayerDataManager.Instance.GetSoundSetting();
    }

    private void UpdateVolumeMusic()
    {
        musicVolume = PlayerDataManager.Instance.GetMusicSetting();
    }

    /// <summary>
    /// Play Sound Effect
    /// </summary>
    /// <param name="clips">Array of sounds</param>
    /// <param name="aus">Audio Source</param>
    public void PlaySound(AudioClip[] clips, AudioSource aus = null)
    {
        if (!aus)
        {
            aus = sfxAus;
        }

        if (clips != null && clips.Length > 0 && aus)
        {
            var randomIdx = Random.Range(0, clips.Length);
            aus.PlayOneShot(clips[randomIdx], sfxVolume);
        }
    }

    /// <summary>
    /// Play Sound Effect
    /// </summary>
    /// <param name="clip">Sounds</param>
    /// <param name="aus">Audio Source</param>
    public void PlaySound(AudioClip clip, AudioSource aus = null)
    {
        if (!aus)
        {
            aus = sfxAus;
        }

        if (clip != null && aus)
        {
            aus.PlayOneShot(clip, sfxVolume);
        }
    }

    /// <summary>
    /// Play Music
    /// </summary>
    /// <param name="musics">Array of musics</param>
    /// <param name="loop">Can Loop</param>
    public void PlayMusic(AudioClip[] musics, bool loop = true)
    {
        if (musicAus && musics != null && musics.Length > 0)
        {
            var randomIdx = Random.Range(0, musics.Length);

            musicAus.clip = musics[randomIdx];
            musicAus.loop = loop;
            musicAus.volume = musicVolume;
            musicAus.Play();
        }
    }

    /// <summary>
    /// Play Music
    /// </summary>
    /// <param name="music">music</param>
    /// <param name="canLoop">Can Loop</param>
    public void PlayMusic(AudioClip music, bool canLoop)
    {
        if (musicAus && music != null)
        {
            musicAus.clip = music;
            musicAus.loop = canLoop;
            musicAus.volume = musicVolume;
            musicAus.Play();
        }
    }

    /// <summary>
    /// Set volume for audiosource
    /// </summary>
    /// <param name="vol">New Volume</param>
    public void SetMusicVolume(float vol)
    {
        musicAus.volume = vol;
    }

    public void SetSoundVolume(float vol)
    {
        sfxAus.volume = vol;
    }

    /// <summary>
    /// Stop play music or sound effect
    /// </summary>
    public void StopPlayMusic()
    {
        if (musicAus) musicAus.Stop();
    }

    public void PlayBackgroundMusic()
    {
        PlayMusic(backgroundMusics, true);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventConstants.UPDATE_VOLUME_MUSIC, UpdateVolumeMusic);
        EventManager.StopListening(EventConstants.UPDATE_VOLUME_SOUND, UpdateVolumeSound);
    }
}
