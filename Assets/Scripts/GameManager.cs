using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────
    public static GameManager Instance
    { get; private set; }

    [Header("Game Settings")]
    public int totalLives = 3;

    [Header("References")]
    public CrocodileController crocodile;
    public FrogController frog;
    public QuestionDatabase questionDatabase;
    public LilyPad[] lilyPads;

    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    public LivesDisplay livesDisplay;
    public GameOverPanel gameOverPanel;

    [Header("Pad Settings")]
    public float padEntryStartY = 8f;
    public float padEntryDuration = 0.8f;
    public float rideBackDuration = 0.6f;
    public float fadeOutDuration = 0.5f;

    // ── Pad original positions ────────────────────
    private Vector3[] padOriginalPositions;

    // ── Private State ─────────────────────────────
    private int currentLives;
    private int score = 0;
    private bool gameActive = false;
    private int currentQuestionIndex = 0;
    private int[] questionOrder;
    private QuestionData currentQuestion;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Save original pad positions
        padOriginalPositions = new Vector3[lilyPads.Length];
        for (int i = 0; i < lilyPads.Length; i++)
        {
            padOriginalPositions[i] =
                lilyPads[i].transform.position;
            lilyPads[i].SetTargetPosition(
                padOriginalPositions[i]);
        }

        currentLives = totalLives;
        gameActive = true;

        questionDatabase.LoadQuestions();
        ShuffleQuestions();

        UpdateLivesUI();
        scoreText.text = "Score: 0";

        // Start game
        StartCoroutine(EnterPadsAndLoadQuestion());
    }

    // ── Shuffle questions ─────────────────────────
    private void ShuffleQuestions()
    {
        int count = questionDatabase.questions.Length;
        questionOrder = new int[count];

        for (int i = 0; i < count; i++)
            questionOrder[i] = i;

        for (int i = count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = questionOrder[i];
            questionOrder[i] = questionOrder[j];
            questionOrder[j] = temp;
        }
    }

    // ── Load question text only ───────────────────
    private void LoadQuestion()
    {
        currentQuestion =
            questionDatabase.questions[
                questionOrder[currentQuestionIndex]];

        questionText.text =
            currentQuestion.questionText;

        Debug.Log("Loading question: " +
                  currentQuestion.questionText);
    }

    // ── Pads enter from top ───────────────────────
    private IEnumerator EnterPadsAndLoadQuestion()
    {
        // Load question first
        LoadQuestion();

        // Reset all pads and set text BEFORE entry
        for (int i = 0; i < lilyPads.Length; i++)
        {
            lilyPads[i].ResetPad();
            lilyPads[i].SetClickable(false);

            // Set text after reset
            lilyPads[i].SetAnswerText(
                currentQuestion.answers[i]);

            // Place above screen
            lilyPads[i].transform.position =
                new Vector3(
                    padOriginalPositions[i].x,
                    padEntryStartY,
                    padOriginalPositions[i].z);
        }

        // Small delay before animating
        yield return new WaitForSeconds(0.1f);

        // Animate all 4 pads entering
        for (int i = 0; i < lilyPads.Length; i++)
        {
            Vector3 startPos = new Vector3(
                padOriginalPositions[i].x,
                padEntryStartY,
                padOriginalPositions[i].z);

            StartCoroutine(lilyPads[i].EnterFromTop(
                startPos,
                padOriginalPositions[i],
                padEntryDuration));

            yield return new WaitForSeconds(0.15f);
        }

        // Wait for all to finish
        yield return new WaitForSeconds(
            padEntryDuration + 0.2f);

        // Enable clicking
        foreach (var pad in lilyPads)
        {
            pad.SetClickable(true);
            Debug.Log("Pad " + pad.padIndex
                      + " now clickable");
        }
    }

    // ── Called by FrogController after landing ────
    public void OnPadLanded(int padIndex)
    {
        if (!gameActive) return;
        StartCoroutine(HandleAnswer(padIndex));
    }

    private IEnumerator HandleAnswer(int padIndex)
{
    bool isCorrect =
        padIndex == currentQuestion.correctIndex;

    if (isCorrect)
    {
        score += 100;
        scoreText.text = "Score: " + score;
        lilyPads[padIndex].FlashCorrect();
        crocodile.PushBack();

        // ── Play correct sound ─────────────────
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayCorrectAnswer();
    }
    else
    {
        lilyPads[padIndex].FlashWrong();
        lilyPads[currentQuestion.correctIndex]
            .FlashCorrect();
        currentLives--;
        UpdateLivesUI();
        crocodile.Lunge();

        // ── Play wrong sound ───────────────────
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayWrongAnswer();

        if (currentLives <= 0)
        {
            yield return new WaitForSeconds(0.8f);
            GameOver();
            yield break;
        }
    }

    yield return new WaitForSeconds(0.8f);

    for (int i = 0; i < lilyPads.Length; i++)
    {
        if (i != padIndex)
            StartCoroutine(
                lilyPads[i].FadeOut(fadeOutDuration));
    }

    // ── Play pad fade sound ────────────────────
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayPadFade();

    yield return StartCoroutine(
        frog.RideBackOnPad(
            lilyPads[padIndex],
            rideBackDuration));

    yield return new WaitForSeconds(0.3f);

    currentQuestionIndex++;
    if (currentQuestionIndex >=
        questionOrder.Length)
    {
        ShuffleQuestions();
        currentQuestionIndex = 0;
    }

    StartCoroutine(EnterPadsAndLoadQuestion());
}

    // ── Called by CrocodileController ─────────────
    public void LoseLife()
    {
        if (!gameActive) return;
        GameOver();
    }

    // ── Game Over ─────────────────────────────────
    private void GameOver()
{
    gameActive = false;
    frog.DisableJump();
    crocodile.ResetPosition();

    foreach (var pad in lilyPads)
        pad.SetClickable(false);

    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayGameOver();

    PlayerPrefs.SetInt("FinalScore", score);
    PlayerPrefs.Save();
    gameOverPanel.Show(score);
}
    private void UpdateLivesUI()
    {
        livesDisplay.UpdateHearts(currentLives);
    }

    public bool IsGameActive()
    {
        return gameActive;
    }

    public int GetLives()
    {
        return currentLives;
    }

    public QuestionData GetCurrentQuestion()
    {
        return currentQuestion;
    }
}