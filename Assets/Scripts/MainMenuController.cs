using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject ftuePanel;

    [Header("Music Button Images")]
    public Image musicButtonImage;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    [Header("Settings")]
    public float dropdownSpeed = 0.3f;

    // ── Private State ─────────────────────────────
    private bool settingsOpen = false;
    private bool musicOn = true;
    private Vector3 settingsPanelHiddenScale;
    private Vector3 settingsPanelShownScale;

    void Start()
    {
        settingsPanel.SetActive(false);
        ftuePanel.SetActive(false);

        settingsPanelHiddenScale =
            new Vector3(1f, 0f, 1f);
        settingsPanelShownScale = Vector3.one;

        // Load saved music preference
        musicOn = PlayerPrefs.GetInt(
            "MusicOn", 1) == 1;
        AudioListener.volume = musicOn ? 1f : 0f;
        UpdateMusicButton();
    }

    // ── Play Button ───────────────────────────────
    public void OnPlayClicked()
{
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayButtonClick();

    if (SceneFader.Instance != null)
        SceneFader.Instance.FadeTo("GameScene");
    else
        SceneManager.LoadScene("GameScene");
}



    // ── Info Button ───────────────────────────────
    public void OnInfoClicked()
{
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayButtonClick();
    ftuePanel.SetActive(true);
}

    // ── Got It Button ─────────────────────────────
    public void OnGotItClicked()
{
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayButtonClick();
    ftuePanel.SetActive(false);
}
    // ── Settings Button ───────────────────────────
    public void OnSettingsClicked()
{
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayButtonClick();

    if (settingsOpen)
        StartCoroutine(CloseSettings());
    else
        StartCoroutine(OpenSettings());
}

    // ── Open dropdown ─────────────────────────────
    private IEnumerator OpenSettings()
    {
        settingsOpen = true;
        settingsPanel.SetActive(true);

        settingsPanel.transform.localScale =
            settingsPanelHiddenScale;

        float elapsed = 0f;
        while (elapsed < dropdownSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dropdownSpeed;
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            settingsPanel.transform.localScale =
                Vector3.Lerp(
                    settingsPanelHiddenScale,
                    settingsPanelShownScale,
                    smooth);

            yield return null;
        }

        settingsPanel.transform.localScale =
            settingsPanelShownScale;
    }

    // ── Close dropdown ────────────────────────────
    private IEnumerator CloseSettings()
    {
        float elapsed = 0f;
        while (elapsed < dropdownSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dropdownSpeed;
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            settingsPanel.transform.localScale =
                Vector3.Lerp(
                    settingsPanelShownScale,
                    settingsPanelHiddenScale,
                    smooth);

            yield return null;
        }

        settingsPanel.SetActive(false);
        settingsOpen = false;
    }

    // ── Music Button ──────────────────────────────
    public void OnMusicClicked()
{
    musicOn = !musicOn;
    PlayerPrefs.SetInt("MusicOn",
        musicOn ? 1 : 0);

    if (AudioManager.Instance != null)
        AudioManager.Instance.ToggleMusic(musicOn);

    UpdateMusicButton();
}

    private void UpdateMusicButton()
    {
        // Swap sprite based on state
        if (musicButtonImage != null)
        {
            musicButtonImage.sprite = musicOn
                ? musicOnSprite
                : musicOffSprite;
        }
    }

    // ── Exit Button ───────────────────────────────
    public void OnExitClicked()
{
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayButtonClick();

    Application.Quit();
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying
        = false;
#endif
}
}