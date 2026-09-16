using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────
    public static AudioManager Instance
    { get; private set; }

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("Game Sound Effects")]
    public AudioClip frogJumpSound;
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;
    public AudioClip crocodileLungeSound;
    public AudioClip padEnterSound;
    public AudioClip padFadeSound;

    [Header("UI Sound Effects")]
    public AudioClip buttonClickSound;
    public AudioClip gameOverSound;
    public AudioClip padFlashSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    // ── Private Sources ───────────────────────────
    private AudioSource musicSource;
    private AudioSource sfxSource;
    private bool musicOn = true;

    // ── Unity Callbacks ───────────────────────────
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create two audio sources
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource   = gameObject.AddComponent<AudioSource>();

        // Music source settings
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        // SFX source settings
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;

        // Load saved preference
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        AudioListener.volume = musicOn ? 1f : 0f;
    }

    void Start()
    {
        PlayMusic();
    }

    // ── Music Controls ────────────────────────────
    public void PlayMusic()
    {
        if (backgroundMusic == null) return;
        if (musicSource.isPlaying) return;

        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void ToggleMusic(bool on)
    {
        musicOn = on;
        AudioListener.volume = on ? 1f : 0f;
        PlayerPrefs.SetInt("MusicOn", on ? 1 : 0);
    }

    // ── SFX Helpers ───────────────────────────────
    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // ── Game Sound Effects ────────────────────────
    public void PlayFrogJump()
    {
        PlaySFX(frogJumpSound);
    }

    public void PlayCorrectAnswer()
    {
        PlaySFX(correctAnswerSound);
    }

    public void PlayWrongAnswer()
    {
        PlaySFX(wrongAnswerSound);
    }

    public void PlayCrocodileLunge()
    {
        PlaySFX(crocodileLungeSound);
    }

    public void PlayPadEnter()
    {
        PlaySFX(padEnterSound);
    }

    public void PlayPadFade()
    {
        PlaySFX(padFadeSound);
    }

    // ── UI Sound Effects ──────────────────────────
    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }

    public void PlayGameOver()
    {
        PlaySFX(gameOverSound);
        StopMusic();
    }

    public void PlayPadFlash()
    {
        PlaySFX(padFlashSound);
    }
}