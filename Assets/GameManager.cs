using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalScoreText;
    public GameObject startPanel;
    public GameObject gameOverPanel;

    [Header("Settings")]
    public float gameDuration = 30f;
    public int startingLives = 3;

    [Header("Sounds")]
    public AudioClip hitSound;
    public AudioClip missSound;
    public AudioClip gameOverSound;

    public bool IsPlaying { get; private set; }

    int score, lives;
    float timeLeft;
    AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        IsPlaying = false;
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        score = 0;
        lives = startingLives;
        timeLeft = gameDuration;
        UpdateUI();
    }

    public void StartGame()
    {
        score = 0;
        lives = startingLives;
        timeLeft = gameDuration;
        IsPlaying = true;
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (!IsPlaying) return;

        // Countdown timer
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            UpdateUI();
            EndGame();
            return;
        }

        // Click / tap handling
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPoint);

            if (hit != null && hit.TryGetComponent(out Frog frog))
            {
                frog.Kill();
                score += 10;
                Play(hitSound);
            }
            else
            {
                score = Mathf.Max(0, score - 5);   // wrong click penalty
                Play(missSound);
            }
        }

        UpdateUI();
    }

    public void FrogEscaped()
    {
        if (!IsPlaying) return;
        lives--;
        Play(missSound);
        UpdateUI();
        if (lives <= 0) EndGame();
    }

    void EndGame()
    {
        IsPlaying = false;
        finalScoreText.text = "Final Score: " + score;
        gameOverPanel.SetActive(true);
        Play(gameOverSound);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
    }

    void Play(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
    }
}