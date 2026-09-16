using UnityEngine;

public class CrocodileController : MonoBehaviour
{
    [Header("Movement")]
    public float constantSpeed = 0.5f;
    public float lungeDistance = 0.8f;
    public float pushBackDistance = 1.5f;
    public float lungeSpeed = 5f;
    public float pushBackSpeed = 3f;

    [Header("Boundaries")]
    public float minY = -4.3f;
    public float maxY = -2.5f;

    // ── Private State ─────────────────────────────
    private float targetY;
    private bool isMovingToTarget = false;
    private float currentMoveSpeed = 0f;

    void Start()
    {
        targetY = transform.position.y;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (!GameManager.Instance.IsGameActive())
            return;

        if (isMovingToTarget)
        {
            float newY = Mathf.MoveTowards(
                transform.position.y,
                targetY,
                currentMoveSpeed * Time.deltaTime);

            transform.position = new Vector3(
                transform.position.x,
                newY,
                transform.position.z);

            if (Mathf.Abs(
                transform.position.y - targetY)
                < 0.05f)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    targetY,
                    transform.position.z);
                isMovingToTarget = false;
            }
        }
        else
        {
            float newY = transform.position.y +
                         constantSpeed * Time.deltaTime;
            newY = Mathf.Min(newY, maxY);

            transform.position = new Vector3(
                transform.position.x,
                newY,
                transform.position.z);

            // Touches frog = instant game over
            if (transform.position.y >= maxY)
            {
                GameManager.Instance.LoseLife();
                ResetPosition();
            }
        }
    }

    public void PushBack()
    {
        targetY = Mathf.Max(
            transform.position.y - pushBackDistance,
            minY);
        currentMoveSpeed = pushBackSpeed;
        isMovingToTarget = true;
    }

    public void Lunge()
{
    targetY = Mathf.Min(
        transform.position.y + lungeDistance,
        maxY);
    currentMoveSpeed = lungeSpeed;
    isMovingToTarget = true;

    // ── Play croc lunge sound ──────────────────
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayCrocodileLunge();
}

    public void ResetPosition()
    {
        isMovingToTarget = false;
        transform.position = new Vector3(
            transform.position.x,
            minY,
            transform.position.z);
        targetY = minY;
    }
}