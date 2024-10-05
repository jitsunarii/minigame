using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System;
using UnityEngine.UI;


public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance { get; private set; }
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Transform leaderboardContent;
    [SerializeField] private GameObject leaderboardEntryPrefab;
    private string playerId;

    private void Awake()
    {
        InitializeSingleton();
    }
    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void SetPlayerName(string name, Action<bool> callback)
    {
        Debug.Log("Setting player name...");
        LoginWithCustomIdAndSetName(name, callback);
    }

    private void LoginWithCustomIdAndSetName(string name, Action<bool> callback)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier + name,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request,
            result => UpdateDisplayName(result, name, callback),
            error => {
                Debug.LogError($"Login failed during name update: {error.ErrorMessage}");
                callback(false);
            });
    }

    private void UpdateDisplayName(LoginResult loginResult, string name, Action<bool> callback)
    {
        playerId = loginResult.PlayFabId;
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            result => {
                Debug.Log($"Display name updated to: {result.DisplayName}");
                callback(true);
            },
            error => {
                Debug.LogError($"Display name update failed: {error.ErrorMessage}");
                callback(false);
            });
    }

    public void GetPlayerName(Action<string> callback)
    {
        var request = new GetPlayerProfileRequest
        {
            PlayFabId = playerId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true
            }
        };
        PlayFabClientAPI.GetPlayerProfile(request,
            result => {
                string name = result.PlayerProfile?.DisplayName ?? "";
                callback(name);
            },
            error => {
                Debug.LogError($"Get player name failed: {error.ErrorMessage}");
                callback("");
            });
    }

    public void SendLeaderboard(int score, string leaderboardName)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnLeaderboardUpdateFailure);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Leaderboard update successful!");
    }

    private void OnLeaderboardUpdateFailure(PlayFabError error)
    {
        Debug.LogError($"Leaderboard update failed: {error.ErrorMessage}");
    }

    public void RequestLeaderboard() {
        Debug.Log("Requesting leaderboard...");
    //       if (string.IsNullOrEmpty(playerId))
    // {
    //     Debug.LogError("Player ID is null or empty. Reconnecting to PlayFab...");
    //     ReconnectToPlayFab();
    //     return;
    // }
    PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest {
            StatisticName = "AllTimeHighScore",
            StartPosition = 0,
            MaxResultsCount = 10
    }, result=> DisplayLeaderboard(result), FailureCallback);
}
private void DisplayLeaderboard(GetLeaderboardResult result){
    Debug.Log("Leaderboard received:");

    foreach (Transform child in leaderboardContent)
    {
        Destroy(child.gameObject);
    }
        foreach (var item in result.Leaderboard)
        {
            GameObject entryObject = Instantiate(leaderboardEntryPrefab, leaderboardContent);
            Text[] texts = entryObject.GetComponentsInChildren<Text>();

            if (texts.Length >= 3)
            {
                texts[0].text = (item.Position + 1).ToString();  // 順位
                texts[1].text = item.DisplayName;                // プレイヤー名
                texts[2].text = item.StatValue.ToString();       // スコア
            }
            else
            {
                Debug.LogWarning("Leaderboard entry prefab does not have enough Text components");
            }
        }
        leaderboardPanel.SetActive(true);
    // foreach (var item in result.Leaderboard)
    // {
    //     Debug.Log($"{item.Position+1}位:{item.DisplayName}"+$"スコア {item.StatValue}");
    // }
}
private void FailureCallback(PlayFabError error){
    Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
    Debug.LogError(error.GenerateErrorReport());
}

  public void SetDebugPlayerName(string debugName)
    {
        // デバッグ用の仮のプレイヤー名設定
        // 注意: この方法はテスト用であり、実際のPlayFab接続は行われません
        Debug.Log($"Debug: Setting player name to {debugName}");
        // 必要に応じて、ローカルでプレイヤー名を保存する処理を追加
    }
//     public void ReconnectToPlayFab()
// {
//     if (string.IsNullOrEmpty(playerId))
//     {
//         // プレイヤーIDがない場合は新規ログイン
//         LoginWithCustomIdAndSetName("Player", (success) => {
//             if (success)
//             {
//                 Debug.Log("Reconnected to PlayFab");
//             }
//             else
//             {
//                 Debug.LogError("Failed to reconnect to PlayFab");
//             }
//         });
//     }
//     else
//     {
//         // プレイヤーIDがある場合は既存のIDでログイン
//         var request = new LoginWithCustomIDRequest
//         {
//             CustomId = playerId,
//             CreateAccount = false
//         };
//         PlayFabClientAPI.LoginWithCustomID(request,
//             result => {
//                 Debug.Log("Reconnected to PlayFab");
//             },
//             error => {
//                 Debug.LogError($"Failed to reconnect to PlayFab: {error.ErrorMessage}");
//             });
//     }
// }
}


// using UnityEngine;
// using PlayFab;
// using PlayFab.ClientModels;
// using System.Collections.Generic;
// using System;

// public class PlayfabManager : MonoBehaviour
// {
//      public static PlayfabManager Instance { get; private set; }
//     public event Action LoginSuccessEvent;

//     private string playerId;

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void Start()
//     {
//         // LoginSuccessEvent?.Invoke();
//     }

//     private void Login()
//     {
//         var request = new LoginWithCustomIDRequest
//         {
//             CustomId = SystemInfo.deviceUniqueIdentifier,
//             CreateAccount = true
//         };
//         PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
//     }

//    private void OnLoginSuccess(LoginResult result)
//     {
//         playerId = result.PlayFabId;
//         Debug.Log("Playfab login success!");
//         LoginSuccessEvent?.Invoke();
//     }

//     private void OnGetPlayerNameAfterLogin(string playerName)
//     {
//         if (!string.IsNullOrEmpty(playerName))
//         {
//             Debug.Log(playerName);
//         }
//         else
//         {
//             Debug.Log("名前がありません");
//         }
//     }

//     private void OnLoginFailure(PlayFabError error)
//     {
//         Debug.LogError(error.ErrorMessage);
//     }

//     public void SetPlayerName(string name, Action<bool> callback)
//     {
//         Debug.Log("Setting player name...");
//           var request = new LoginWithCustomIDRequest
//         {
//             CustomId = SystemInfo.deviceUniqueIdentifier+name,
//             CreateAccount = true
//         };
//         PlayFabClientAPI.LoginWithCustomID(request, (LoginResult result) => {
// 	playerId = result.PlayFabId;
// 	       var request2 = new UpdateUserTitleDisplayNameRequest
//         {
//             DisplayName = name
//         };
//         PlayFabClientAPI.UpdateUserTitleDisplayName(request2,
//             result => {
//                 Debug.Log($"Display name updated to: {result.DisplayName}");
//                 callback(true);
//             }, 
//             error => {
//                 Debug.LogError($"Display name update failed: {error.ErrorMessage}");
//                 callback(false);
//             });
// }, OnLoginFailure);

//         //  var request2 = new UpdateUserTitleDisplayNameRequest
//         // {
//         //     DisplayName = name
//         // };
//         // PlayFabClientAPI.UpdateUserTitleDisplayName(request2,
//         //     result => {
//         //         Debug.Log($"Display name updated to: {result.DisplayName}");
//         //         callback(true);
//         //     }, 
//         //     error => {
//         //         Debug.LogError($"Display name update failed: {error.ErrorMessage}");
//         //         callback(false);
//         //     });
//     }

//     public void GetPlayerName(Action<string> callback)
//     {
//         PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
//         {
//             PlayFabId = playerId,
//             ProfileConstraints = new PlayerProfileViewConstraints()
//             {
//                 ShowDisplayName = true
//             }
//         }, 
//         result => {
//             string name = result.PlayerProfile != null ? result.PlayerProfile.DisplayName : "";
//             callback(name);
//         }, 
//         error => {
//             Debug.LogError($"Get player name failed: {error.ErrorMessage}");
//             callback("");
//         });
//     }

//     private void OnGetPlayerNameSuccess(GetPlayerProfileResult result)
//     {
//         if (result.PlayerProfile != null && !string.IsNullOrEmpty(result.PlayerProfile.DisplayName))
//         {
//             Debug.Log($"Player name: {result.PlayerProfile.DisplayName}");
//         }
//         else
//         {
//             Debug.Log("Player name not set");
//         }
//     }

//     private void OnGetPlayerNameFailure(PlayFabError error)
//     {
//         Debug.LogError($"Get player name failed: {error.ErrorMessage}");
//     }

//     public void SendLeaderboard(int score, string leaderboardName)
//     {
//         var request = new UpdatePlayerStatisticsRequest
//         {
//             Statistics = new List<StatisticUpdate>
//             {
//                 new StatisticUpdate
//                 {
//                     StatisticName = leaderboardName,
//                     Value = score
//                 }
//             }
//         };
//         PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnLeaderboardUpdateFailure);
//     }

//     private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
//     {
//         Debug.Log("Leaderboard update success!");
//     }

//     private void OnLeaderboardUpdateFailure(PlayFabError error)
//     {
//         Debug.LogError($"Leaderboard update failed: {error.ErrorMessage}");
//     }
// }