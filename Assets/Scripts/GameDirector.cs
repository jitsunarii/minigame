using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }
    
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timerText;
    [SerializeField] private float gameDuration = 60f;
    [SerializeField] private UImanager uiManager;
    [SerializeField] private NoteSpawner noteSpawner;

      [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private XRInteractorLineVisual leftLineVisual;
    [SerializeField] private XRInteractorLineVisual rightLineVisual;

    [SerializeField] private bool debugAutoStart = true; // インスペクターで切り替え可能

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

      private void Start()
    {
         if (debugAutoStart)
        {
            Debug.Log("Auto-starting game for debugging");
            StartGameImmediate();
        }
        else
        {
             SetupVRRig();
            Debug.Log("Waiting for player to start game");
        }
       
    }

  private void SetupVRRig()
    {
        if (xrOrigin != null)
        {
            xrOrigin.transform.position = new Vector3(0, 0, -4f); // プレイヤーの開始位置を調整
        }
        else
        {
            Debug.LogError("XROrigin is not assigned in the inspector!");
        }

        if (leftLineVisual != null) leftLineVisual.enabled = false;
        if (rightLineVisual != null) rightLineVisual.enabled = false;
    }

      public void StartGameImmediate()
    {
        // UIの初期化をスキップ
        if (uiManager != null)
        {
            uiManager.SkipUISetup();
        }

        // プレイヤー名を仮設定
        PlayfabManager.Instance.SetDebugPlayerName("TestPlayer");

        // ゲーム開始
        StartGame();
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