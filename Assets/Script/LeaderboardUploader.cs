using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

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
    public string username;
    public float score;

    public void SaveData(float newScore, string newUsername)
    {
        score = newScore;
        username = string.IsNullOrEmpty(newUsername) ? "user1" : newUsername;
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        LeaderBoardEntry entry = new LeaderBoardEntry
        {
            user = username,
            score = score,
            date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            game = "shooting"
        };

        string json = JsonUtility.ToJson(entry);
        
        using (UnityWebRequest www = UnityWebRequest.Post("https://pota.uber.space/api/leaderboard", json, "application/json"))
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
}
