using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────
    public static SceneFader Instance
    { get; private set; }

    [Header("Settings")]
    public float fadeDuration = 0.5f;

    // ── Private ───────────────────────────────────
    private Image fadeImage;
    private Canvas fadeCanvas;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create canvas and image in code
        // so they persist with this object
        CreateFadeCanvas();
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    // ── Create fade canvas in code ────────────────
    private void CreateFadeCanvas()
    {
        // Create canvas
        GameObject canvasGO = new GameObject("FadeCanvas");
        canvasGO.transform.SetParent(transform);

        fadeCanvas = canvasGO.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 999;

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Create image
        GameObject imageGO = new GameObject("FadeImage");
        imageGO.transform.SetParent(
            canvasGO.transform, false);

        fadeImage = imageGO.AddComponent<Image>();
        fadeImage.color = Color.black;

        // Stretch to fill
        RectTransform rt =
            imageGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        fadeImage.gameObject.SetActive(false);
    }

    // ── Fade in (black to clear) ──────────────────
    public IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        fadeImage.gameObject.SetActive(true);
        fadeImage.color = Color.black;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            fadeImage.color = new Color(
                0f, 0f, 0f, 1f - t);
            yield return null;
        }

        fadeImage.color = Color.clear;
        fadeImage.gameObject.SetActive(false);
    }

    // ── Fade out then load scene ──────────────────
    public void FadeTo(string sceneName)
    {
        StartCoroutine(FadeOutRoutine(sceneName));
    }

    private IEnumerator FadeOutRoutine(string sceneName)
    {
        if (fadeImage == null) yield break;

        fadeImage.gameObject.SetActive(true);
        fadeImage.color = Color.clear;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            fadeImage.color = new Color(
                0f, 0f, 0f, t);
            yield return null;
        }

        fadeImage.color = Color.black;
        SceneManager.LoadScene(sceneName);

        // Fade in after loading
        yield return StartCoroutine(FadeIn());
    }
}