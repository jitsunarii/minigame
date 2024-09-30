using UnityEngine;
using UnityEngine.UI;
using System;

public class UImanager : MonoBehaviour
{
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private Button setNameButton;
    [SerializeField] private Text currentNameText;
    [SerializeField] private Text feedbackText;
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text resultScoreText;

    private void Start()
    {
        InitializeUI();
        SetInitialPanelStates();
    }

    private void InitializeUI()
    {
        setNameButton.onClick.AddListener(SetPlayerNameAndStartGame);
    }

    private void SetInitialPanelStates()
    {
        SetPanelActive(nameInputPanel, true);
        SetPanelActive(gameplayPanel, false);
        SetPanelActive(resultPanel, false);
    }

    private void SetPlayerNameAndStartGame()
    {
        string newName = playerNameInput.text.Trim();
        if (string.IsNullOrEmpty(newName))
        {
            SetFeedbackText("Please enter a valid name.");
            return;
        }

        SetFeedbackText("Setting player name...");
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
            SetFeedbackText("Failed to set name. Please try again.");
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
        resultScoreText.text = $"Final Score: {score}";
    }

    private void SetPanelActive(GameObject panel, bool active)
    {
        panel.SetActive(active);
    }

    private void SetFeedbackText(string message)
    {
        feedbackText.text = message;
    }
}

// using UnityEngine;
// using UnityEngine.UI;
// using PlayFab;
// using PlayFab.ClientModels;
// using System.Collections.Generic;
// using System;

// public class UImanager : MonoBehaviour
// {
//     [SerializeField] private InputField playerNameInput;
//     [SerializeField] private Button setNameButton;
//     [SerializeField] private Text currentNameText;
//     [SerializeField] private Text feedbackText;
//     [SerializeField] private GameObject nameInputPanel;
//     [SerializeField] private GameObject gameplayPanel;
//     [SerializeField] private GameObject resultPanel;
//     [SerializeField] private Text resultScoreText;

//     private void Start()
//     {
//         InitializeUI();
//         // PlayfabManager.Instance.LoginSuccessEvent += InitializeUI;
//         SetNameInputPanelActive(true);
//         SetGameplayPanelActive(false);
//         SetResultPanelActive(false);
//     }

//     private void InitializeUI()
//     {
//         setNameButton.onClick.AddListener(SetPlayerNameAndStartGame);
//         UpdateCurrentNameText();
//     }

//     private void OnDestroy()
//     {
//         if (PlayfabManager.Instance != null)
//         {
//             PlayfabManager.Instance.LoginSuccessEvent -= InitializeUI;
//         }
//     }

//     private void SetPlayerNameAndStartGame()
//     {
//         string newName = playerNameInput.text.Trim();
//         if (!string.IsNullOrEmpty(newName))
//         {
//             PlayfabManager.Instance.SetPlayerName(newName, OnPlayerNameSet);
//             feedbackText.text = "Setting player name...";
//         }
//         else
//         {
//             feedbackText.text = "Please enter a valid name.";
//         }
//     }

//     private void OnPlayerNameSet(bool success)
//     {
//         if (success)
//         {
//             Debug.Log("Player name set successfully");
//             SetNameInputPanelActive(false);
//             SetGameplayPanelActive(true);
//             GameDirector.Instance.StartGame();
//         }
//         else
//         {
//             Debug.LogError("Failed to set player name");
//             feedbackText.text = "Failed to set name. Please try again.";
//         }
//     }

//     private void UpdateCurrentNameText()
//     {
//         // PlayfabManager.Instance.GetPlayerName(OnGetPlayerName);
//     }

//     private void OnGetPlayerName(string playerName)
//     {
//         if (!string.IsNullOrEmpty(playerName))
//         {
//             currentNameText.text = $"Current Name: {playerName}";
//         }
//         else
//         {
//             currentNameText.text = "No name set";
//         }
//     }

//     public void ShowResult(int score)
//     {
//         SetGameplayPanelActive(false);
//         SetResultPanelActive(true);
//         resultScoreText.text = $"Final Score: {score}";
//     }

//     private void SetNameInputPanelActive(bool active)
//     {
//         nameInputPanel.SetActive(active);
//     }

//     private void SetGameplayPanelActive(bool active)
//     {
//         gameplayPanel.SetActive(active);
//     }

//     private void SetResultPanelActive(bool active)
//     {
//         resultPanel.SetActive(active);
//     }
// }


// public class UImanager : MonoBehaviour
// {
//     [SerializeField] private InputField playerNameInput;
//     [SerializeField] private Button setNameButton;
//     [SerializeField] private Text currentNameText;
// [SerializeField] private Text feedbackText;
//     private void Start()
//     {
//         PlayfabManager.Instance.LoginSuccessEvent += InitializeUI;
//     }

//     private void InitializeUI()
//     {
//         setNameButton.onClick.AddListener(SetPlayerName);
//         UpdateCurrentNameText();
//     }

//     private void OnDestroy()
//     {
//         if (PlayfabManager.Instance != null)
//         {
//             PlayfabManager.Instance.LoginSuccessEvent -= InitializeUI;
//         }
//     }

//     public void SetPlayerName()
//     {
//         string newName = playerNameInput.text.Trim();
//         if (!string.IsNullOrEmpty(newName))
//         {
//             PlayfabManager.Instance.SetPlayerName(newName, OnPlayerNameSet);
//             feedbackText.text = "Setting player name...";
//         }
//         else
//         {
//             feedbackText.text = "Please enter a valid name.";
//         }
//     }

//     private void OnPlayerNameSet(bool success)
//     {
//         if (success)
//         {
//             Debug.Log("Player name set successfully");
//             UpdateCurrentNameText();
//             playerNameInput.text = ""; // Clear input field
//         }
//         else
//         {
//             Debug.LogError("Failed to set player name");
//         }
//     }

//     private void UpdateCurrentNameText()
//     {
//         PlayfabManager.Instance.GetPlayerName(OnGetPlayerName);
//     }

//     private void OnGetPlayerName(string playerName)
//     {
//         if (!string.IsNullOrEmpty(playerName))
//         {
//             currentNameText.text = $"Current Name: {playerName}";
//         }
//         else
//         {
//             currentNameText.text = "No name set";
//         }
//     }
// }