using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI factText;

    // ── Random facts about periodic table ─────────
    private string[] facts = new string[]
    {
        "Hydrogen is the most abundant element in the universe!",
        "Gold is so rare that more steel is poured in an hour than gold has been poured since the beginning of time!",
        "Helium is the only element that was discovered in space before it was found on Earth!",
        "The human body contains enough carbon to make 900 pencils!",
        "Oxygen was discovered by Carl Wilhelm Scheele in 1772!",
        "Mercury is the only metal that is liquid at room temperature!",
        "Tungsten has the highest melting point of all elements at 3,422°C!",
        "There are 118 confirmed elements in the periodic table!",
        "Gallium melts in your hand because its melting point is just above room temperature!",
        "Plutonium glows blue in the dark due to its radioactivity!",
        "The periodic table was created by Dmitri Mendeleev in 1869!",
        "Francium is the rarest naturally occurring element on Earth!",
        "Osmium is the densest naturally occurring element!",
        "Carbon can exist as both graphite and diamond — two very different forms!",
        "Neon signs are not always filled with Neon — different gases create different colors!"
    };

    // ── Show the panel ────────────────────────────
    public void Show(int score)
    {
        gameObject.SetActive(true);
        finalScoreText.text = "Score: " + score;
        factText.text = "Did you know?\n" + 
                        facts[Random.Range(0, facts.Length)];
    }

    // ── Button functions ──────────────────────────
    public void OnRetryClicked()
{
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayButtonClick();

    if (SceneFader.Instance != null)
        SceneFader.Instance.FadeTo("GameScene");
    else
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
}
    public void OnMenuClicked()
{
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayButtonClick();

    if (SceneFader.Instance != null)
        SceneFader.Instance.FadeTo("MainMenu");
    else
        SceneManager.LoadScene("MainMenu");
}
}