using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpDuration = 0.4f;
    public float jumpHeight = 1.5f;
    public float squishAmount = 0.15f;

    [Header("Pad References")]
    public LilyPad[] lilyPads;

    // ── Private State ─────────────────────────────
    private Vector3 startPosition;
    private Vector3 originalScale;
    private bool isJumping = false;
    private bool canJump = true;

    void Start()
    {
        startPosition = transform.position;
        originalScale = transform.localScale;
    }

    // ── Called when player taps a pad ─────────────
    public void JumpToPad(int padIndex)
    {
        if (!canJump || isJumping) return;
        if (!GameManager.Instance.IsGameActive()) return;
        if (padIndex < 0 ||
            padIndex >= lilyPads.Length) return;
        if (!lilyPads[padIndex].IsClickable()) return;

        StartCoroutine(JumpArc(
            lilyPads[padIndex].transform.position,
            padIndex));
    }

    // ── Parabolic jump arc ────────────────────────
    private IEnumerator JumpArc(Vector3 target,
                            int padIndex)
{
    isJumping = true;
    canJump = false;

    foreach (var pad in lilyPads)
        pad.SetClickable(false);

    // ── Play jump sound ────────────────────────
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayFrogJump();

    Vector3 origin = transform.position;
    float elapsed = 0f;

    StartCoroutine(SquishAnim(true));

    while (elapsed < jumpDuration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / jumpDuration;

        Vector3 flatPos = Vector3.Lerp(
            origin, target, t);
        float height = Mathf.Sin(t * Mathf.PI)
                       * jumpHeight;

        transform.position = new Vector3(
            flatPos.x,
            flatPos.y + height,
            flatPos.z);

        yield return null;
    }

    transform.position = target;
    StartCoroutine(SquishAnim(false));
    isJumping = false;

    GameManager.Instance.OnPadLanded(padIndex);
}

    // ── Ride back sitting on pad ──────────────────
    public IEnumerator RideBackOnPad(LilyPad pad,
                                 float duration)
{
    Vector3 destination = startPosition;
    Vector3 padOrigin = pad.transform.position;
    float elapsed = 0f;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float smooth = Mathf.SmoothStep(
            0f, 1f, elapsed / duration);

        Vector3 newPos = Vector3.Lerp(
            padOrigin, destination, smooth);

        // Move frog and pad together
        transform.position = newPos;
        pad.transform.position = newPos;

        yield return null;
    }

    // Snap to start
    transform.position = startPosition;
    pad.transform.position = startPosition;

    // Now fade out the pad the frog rode on
    yield return StartCoroutine(
        pad.FadeOut(0.3f));

    isJumping = false;
    canJump = true;
}

    // ── Squish animation ──────────────────────────
    private IEnumerator SquishAnim(bool isLaunch)
    {
        float dur = 0.1f;
        float t = 0f;

        Vector3 squish = isLaunch
            ? new Vector3(
                originalScale.x * (1 + squishAmount),
                originalScale.y * (1 - squishAmount),
                1)
            : new Vector3(
                originalScale.x * (1 + squishAmount
                                   * 1.5f),
                originalScale.y * (1 - squishAmount
                                   * 0.5f),
                1);

        while (t < dur)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(
                originalScale, squish, t / dur);
            yield return null;
        }

        t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(
                squish, originalScale, t / dur);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    public void DisableJump()
    {
        canJump = false;
        isJumping = false;
    }

    public void EnableJump()
    {
        canJump = true;
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
}