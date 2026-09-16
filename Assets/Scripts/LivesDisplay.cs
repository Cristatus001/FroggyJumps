using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    [Header("Heart Images")]
    public Image heart1;
    public Image heart2;
    public Image heart3;

    private Color activeColor   = Color.red;
    private Color inactiveColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);

    // ── Call this whenever lives change ───────────
    public void UpdateHearts(int currentLives)
    {
        heart1.color = currentLives >= 1 ? activeColor : inactiveColor;
        heart2.color = currentLives >= 2 ? activeColor : inactiveColor;
        heart3.color = currentLives >= 3 ? activeColor : inactiveColor;
    }
}