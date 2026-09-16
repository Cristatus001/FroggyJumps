using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class LilyPad : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    public TextMeshPro answerText;
    public SpriteRenderer padRenderer;
    public int padIndex;

    [Header("Bob Settings")]
    public float bobSpeed = 1f;
    public float bobAmount = 0.08f;
    public float bobOffsetX = 0.05f;

    // ── Private State ─────────────────────────────
    private Color originalColor;
    private bool isClickable = false;
    private Vector3 targetPosition;
    private bool isBobbing = false;
    private float bobTimeOffset;
    private Vector3 originalScale;

    void Awake()
{
    originalColor = padRenderer.color;
    bobTimeOffset = Random.Range(0f, 2f * Mathf.PI);
    originalScale = transform.localScale; // ← add this
}    void Update()
    {
        if (!isBobbing) return;

        float t = Time.time * bobSpeed + bobTimeOffset;
        float offsetY = Mathf.Sin(t) * bobAmount;
        float offsetX = Mathf.Cos(t * 0.7f) * bobOffsetX;

        transform.position = new Vector3(
            targetPosition.x + offsetX,
            targetPosition.y + offsetY,
            targetPosition.z);
    }

    // ── Setup ─────────────────────────────────────
    public void SetTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
    }

    // ── Called by GameManager ─────────────────────
    public void SetAnswerText(string text)
    {
        Debug.Log("PAD " + padIndex + " text = " + text);
        answerText.text = text;
        answerText.color = Color.white;
        answerText.gameObject.SetActive(true);
    }

    public void SetClickable(bool state)
    {
        isClickable = state;
        Debug.Log("Pad " + padIndex +
                  " clickable: " + state);
    }

    public bool IsClickable()
    {
        return isClickable;
    }

    // ── Click detection ───────────────────────────
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pad " + padIndex + " clicked! " +
                  "Clickable: " + isClickable);
        if (!isClickable) return;
        if (!GameManager.Instance.IsGameActive()) return;
        GameManager.Instance.frog.JumpToPad(padIndex);
    }

    // ── Bobbing ───────────────────────────────────
    public void StartBobbing()
    {
        isBobbing = true;
    }

    public void StopBobbing()
    {
        isBobbing = false;
    }

    // ── Flash correct/wrong ───────────────────────
    public void FlashCorrect()
    {
        StartCoroutine(FlashRoutine(Color.green));
    }

    public void FlashWrong()
    {
        StartCoroutine(FlashRoutine(Color.red));
    }

    private IEnumerator FlashRoutine(Color flashColor)
    {
        padRenderer.color = flashColor;
        yield return new WaitForSeconds(0.8f);
        padRenderer.color = originalColor;
    }

    // ── Fade out 3 wrong pads ─────────────────────
    public IEnumerator FadeOut(float duration)
{
    StopBobbing();
    isBobbing = false;

    Vector3 startScale = transform.localScale; // ← use current scale
    Color startPadColor = padRenderer.color;
    Color startTextColor = answerText.color;

    float elapsed = 0f;
    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / duration;

        transform.localScale = Vector3.Lerp(
            startScale, Vector3.zero, t); // ← lerp from current

        padRenderer.color = new Color(
            startPadColor.r,
            startPadColor.g,
            startPadColor.b,
            1f - t);

        answerText.color = new Color(
            startTextColor.r,
            startTextColor.g,
            startTextColor.b,
            1f - t);

        yield return null;
    }

    gameObject.SetActive(false);
}
    // ── Ride back to start with frog ──────────────
    public IEnumerator RideBack(Vector3 destination,
                                float duration)
    {
        isBobbing = false;
        Vector3 origin = transform.position;
        float elapsed = 0f;

        StartCoroutine(FadeTextOut(duration * 0.5f));

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float smooth = Mathf.SmoothStep(
                0f, 1f, elapsed / duration);

            transform.position = Vector3.Lerp(
                origin, destination, smooth);

            yield return null;
        }

        transform.position = destination;
        targetPosition = destination;
    }

    // ── Fade text out while riding back ───────────
    private IEnumerator FadeTextOut(float duration)
    {
        Color start = answerText.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            answerText.color = new Color(
                start.r, start.g, start.b, 1f - t);
            yield return null;
        }

        answerText.text = "";
        answerText.color = new Color(1f, 1f, 1f, 1f);
    }

    // ── Enter from top with wobble ────────────────
    public IEnumerator EnterFromTop(Vector3 startPos,
                                Vector3 endPos,
                                float duration)
{
    gameObject.SetActive(true);
    isClickable = false;
    isBobbing = false;

    transform.localScale = originalScale;
    padRenderer.color = originalColor;
    answerText.color = Color.white;
    answerText.gameObject.SetActive(true);
    transform.position = startPos;

    // ── Play pad enter sound ───────────────────
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayPadEnter();

    float elapsed = 0f;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / duration;
        float smooth = Mathf.SmoothStep(0f, 1f, t);

        Vector3 basePos = Vector3.Lerp(
            startPos, endPos, smooth);

        float wobble = Mathf.Sin(
            t * Mathf.PI * 4f) * 0.3f * (1f - t);

        transform.position = new Vector3(
            basePos.x + wobble,
            basePos.y,
            basePos.z);

        yield return null;
    }

    transform.position = endPos;
    targetPosition = endPos;
    StartBobbing();

    Debug.Log("Pad " + padIndex + " finished entering");
}

    // ── Reset pad ─────────────────────────────────
    public void ResetPad()
{
    StopAllCoroutines();
    transform.localScale = originalScale; // ← fixed
    padRenderer.color = originalColor;
    answerText.color = Color.white;
    isBobbing = false;
    isClickable = false;
    gameObject.SetActive(true);
}
}