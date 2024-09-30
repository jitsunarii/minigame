using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }
    
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timerText;
    [SerializeField] private float gameDuration = 60f;
    [SerializeField] private UImanager uiManager;
    [SerializeField] private NoteSpawner noteSpawner;

    private int score = 0;
    private float remainingTime;
    private bool isGameActive = false;

    public bool IsGameActive => isGameActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        InitializeGame();
        isGameActive = true;
        noteSpawner.StartSpawning();
    }

    private void InitializeGame()
    {
        score = 0;
        remainingTime = gameDuration;
        UpdateScoreText();
        UpdateTimerText();
    }

    private void Update()
    {
        if (isGameActive)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                EndGame();
            }
        }
    }

    public void AddScore(int points)
    {
        if (isGameActive)
        {
            score += points;
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }

    private void UpdateTimerText()
    {
        int seconds = Mathf.Max(0, Mathf.CeilToInt(remainingTime));
        timerText.text = $"Time: {seconds}s";
    }

    private void EndGame()
    {
        isGameActive = false;
        noteSpawner.StopSpawning();
        Debug.Log($"Game Over! Final Score: {score}");
        
        PlayfabManager.Instance.SendLeaderboard(score, "DailyHighScore");
        PlayfabManager.Instance.SendLeaderboard(score, "WeeklyHighScore");
        PlayfabManager.Instance.SendLeaderboard(score, "AllTimeHighScore");
        
        uiManager.ShowResult(score);
    }
}
// public class GameDirector : MonoBehaviour
// {
//     public static GameDirector Instance { get; private set; }
    
//     [SerializeField] private Text scoreText;
//     [SerializeField] private Text timerText;
//     [SerializeField] private float gameDuration = 60f;

//     private int score = 0;
//     private float remainingTime;

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void Start()
//     {
//         InitializeGame();
//     }

//     private void InitializeGame()
//     {
//         score = 0;
//         remainingTime = gameDuration;
//         UpdateScoreText();
//         UpdateTimerText();
//     }

//     private void Update()
//     {
//         if (remainingTime > 0)
//         {
//             remainingTime -= Time.deltaTime;
//             UpdateTimerText();
//         }
//         else
//         {
//             EndGame();
//         }
//     }

//     public void AddScore(int points)
//     {
//         score += points;
//         UpdateScoreText();
//     }

//     private void UpdateScoreText()
//     {
//         scoreText.text = $"score:{score}";
//     }

//     private void UpdateTimerText()
//     {
//         int seconds = Mathf.Max(0, Mathf.CeilToInt(remainingTime));
//         timerText.text = $"Time: {seconds}s";
//     }

//    private void EndGame()
//     {
//         Debug.Log($"Game Over! Final Score: {score}");
        
//         // Playfabに各リーダーボードにスコアを送信
//         PlayfabManager.Instance.SendLeaderboard(score, "DailyHighScore");
//         PlayfabManager.Instance.SendLeaderboard(score, "WeeklyHighScore");
//         PlayfabManager.Instance.SendLeaderboard(score, "AllTimeHighScore");
        
//         // 現在のシーンをリロード
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     }
// }