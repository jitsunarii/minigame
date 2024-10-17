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
        xrOrigin.transform.position= new Vector3(0, 0, -4f);
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
        xrOrigin.transform.position= new Vector3(0, 0, -4f);
        Debug.Log($"Game Over! Final Score: {score}");
        
        PlayfabManager.Instance.SendLeaderboard(score, "DailyHighScore");
        PlayfabManager.Instance.SendLeaderboard(score, "WeeklyHighScore");
        PlayfabManager.Instance.SendLeaderboard(score, "AllTimeHighScore");
        
        uiManager.ShowResult(score);
    }
    public void ChangeToDemoScene()
    {
        SceneManager.LoadScene("DemoScene");
    }

    public void ChangeToTeleportScene()
    {
        SceneManager.LoadScene("Teleport");
    }

    public void ChangeToTrackingScene()
    {
        SceneManager.LoadScene("HandTracking");
    }
}