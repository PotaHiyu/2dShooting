// using UnityEngine;
// using UnityEngine.Networking;
// using System.Collections;
// using System;
// using System.Collections.Generic;

// [System.Serializable]
// public class LeaderBoardEntry
// {
//     public string user;
//     public float score;
//     public string date;
//     public string game;
// }

// public class LeaderboardUploader : MonoBehaviour
// {
//     public string username;
//     public float score;

//     public void SaveData(float newScore, string newUsername)
//     {
//         score = newScore;
//         username = string.IsNullOrEmpty(newUsername) ? "user1" : newUsername;
//         StartCoroutine(Upload());
//     }
    
//     public IEnumerator UploadWithCallback(float newScore, string newUsername, System.Action<bool, string> callback)
//     {
//         score = newScore;
//         username = string.IsNullOrEmpty(newUsername) ? "user1" : newUsername;
//         yield return StartCoroutine(UploadWithResult(callback));
//     }

//     IEnumerator Upload()
//     {
//         LeaderBoardEntry entry = new LeaderBoardEntry
//         {
//             user = username,
//             score = score,
//             date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
//             game = "shooting"
//         };

//         string json = JsonUtility.ToJson(entry);
        
//         // using (UnityWebRequest www = UnityWebRequest.Post("https://pota.uber.space/api/leaderboard", json, "application/json"))
//         using (UnityWebRequest www = UnityWebRequest.Post("https://potablog.net/api/leaderboard", json, "application/json"))
//         {
//             yield return www.SendWebRequest();

//             if (www.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.LogError("Upload failed: " + www.error);
//             }
//             else
//             {
//                 Debug.Log("Score upload complete!");
//             }
//         }
//     }
    
//     IEnumerator UploadWithResult(System.Action<bool, string> callback)
//     {
//         LeaderBoardEntry entry = new LeaderBoardEntry
//         {
//             user = username,
//             score = score,
//             date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
//             game = "shooting"
//         };

//         string json = JsonUtility.ToJson(entry);
        
//         using (UnityWebRequest www = UnityWebRequest.Post("https://potablog.net/api/leaderboard", json, "application/json"))
//         {
//             yield return www.SendWebRequest();

//             if (www.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.LogError("Upload failed: " + www.error);
//                 callback?.Invoke(false, www.error);
//             }
//             else
//             {
//                 Debug.Log("Score upload complete!");
//                 callback?.Invoke(true, "Success");
//             }
//         }
//     }
// }

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;

[System.Serializable]
public class LeaderBoardEntry
{
    public string user;
    public float score;
    public string date;
    public string game;
}

public class LeaderboardUploader : MonoBehaviour
{
    private int id;
    public string username;
    public float score;

    public void SaveData(float newScore, string newUsername)
    {
        score = newScore;
        username = string.IsNullOrEmpty(newUsername) ? "user" : newUsername;
        StartCoroutine(Upload());
    }
    
    public IEnumerator UploadWithCallback(float newScore, string newUsername, System.Action<bool, string> callback)
    {
        score = newScore;
        username = string.IsNullOrEmpty(newUsername) ? "user" : newUsername;
        yield return StartCoroutine(UploadWithResult(callback));
    }

    IEnumerator Upload()
    {
        LeaderBoardEntry entry = new LeaderBoardEntry
        {
            user = username + id,
            score = score,
            date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            game = "xr",
        };

        string json = JsonUtility.ToJson(entry);
        
        // using (UnityWebRequest www = UnityWebRequest.Post("https://pota.uber.space/api/leaderboard", json, "application/json"))
        using (UnityWebRequest www = UnityWebRequest.Post("https://potablog.net/api/leaderboard", json, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Upload failed: " + www.error);
            }
            else
            {
                Debug.Log("Score upload complete!");
            }
        }
    }
    
    IEnumerator UploadWithResult(System.Action<bool, string> callback)
    {
        LeaderBoardEntry entry = new LeaderBoardEntry
        {
            user = username + id,
            score = score,
            date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            game = "xr"
        };

        string json = JsonUtility.ToJson(entry);
        
        using (UnityWebRequest www = UnityWebRequest.Post("https://potablog.net/api/leaderboard", json, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Upload failed: " + www.error);
                callback?.Invoke(false, www.error);
            }
            else
            {
                Debug.Log("Score upload complete!");
                callback?.Invoke(true, "Success");
            }
        }
    }
}
