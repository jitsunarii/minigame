using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class UImanager : MonoBehaviour
{
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private Button setNameButton;
    [SerializeField] private Text currentNameText;
    [SerializeField] private Text feedbackText;
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject resultPanel;

    [SerializeField] private GameObject rankPanel;
    [SerializeField] private Text resultScoreText;

    [SerializeField] private GameObject LeftOptionController;

    [SerializeField] private GameObject RightOptionController;
    [SerializeField] private GameObject LeftGameController;
    [SerializeField] private GameObject RightGameController;

    private void Start()
    {
        SetInitialPanelStates();
    }


    private void SetInitialPanelStates()
    {
        SetPanelActive(nameInputPanel, true);
        SetPanelActive(gameplayPanel, false);
        SetPanelActive(resultPanel, false);
    }

    public void SetPlayerNameAndStartGame()
    {
        string newName = playerNameInput.text.Trim();
        if (string.IsNullOrEmpty(newName))
        {
            SetFeedbackText("Player名を入力してください");
            return;
        }

        SetFeedbackText("ゲームを読み込んでいます...");
        PlayfabManager.Instance.SetPlayerName(newName, OnPlayerNameSet);
        
    }

    private void OnPlayerNameSet(bool success)
    {
        if (success)
        {
            Debug.Log("Player name set successfully");
            TransitionToGameplay();
        }
        else
        {
            Debug.LogError("Failed to set player name");
            SetFeedbackText("読み込みに失敗しました。名前を再設定してください");
            playerNameInput.text = "";

        }
    }

    private void TransitionToGameplay()
    {
        SetPanelActive(nameInputPanel, false);
        SetPanelActive(gameplayPanel, true);
        GameDirector.Instance.StartGame();
    }

    

    private void OnGetPlayerName(string playerName)
    {
        currentNameText.text = string.IsNullOrEmpty(playerName) ? "No name set" : $"Current Name: {playerName}";
    }


    public void ShowResult(int score)
    {
        SetPanelActive(gameplayPanel, false);
        SetPanelActive(resultPanel, true);
        resultScoreText.text = $"スコア: {score}";
    }

    private void SetPanelActive(GameObject panel, bool active)
    {
        panel.SetActive(active);
    }

    private void SetFeedbackText(string message)
    {
        feedbackText.text = message;
    }
      public void SkipUISetup()
    {
        // 必要なUIパネルの非表示化
        SetPanelActive(nameInputPanel, false);
        SetPanelActive(gameplayPanel, true);
        // 他の必要なUI初期化処理
    }
    public void resultPanelRemove()
    {
        SetPanelActive(resultPanel, false);
    }
    public void resultPanelActive()
    {
        SetPanelActive(resultPanel, true);
        SetPanelActive(rankPanel, false);
    }
}