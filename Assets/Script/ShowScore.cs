using UnityEngine;
using TMPro;
using Michsky.MUIP;
using UnityEngine.UI;
using System.Collections;

public class ShowScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private DontDestroyOnLoad dontDestroyObj;
    public TMP_InputField inputField;
    public ButtonManager submitButton;
    private LeaderboardUploader uploader;
    public TextMeshProUGUI notificationText;
    public GameObject notificationPanel;

    void Start()
    {
        dontDestroyObj = GameObject.Find("DontDestroyOnLoad").GetComponent<DontDestroyOnLoad>();
        uploader = gameObject.GetComponent<LeaderboardUploader>();
        scoreText.text = "Score: " + Mathf.FloorToInt(dontDestroyObj.score).ToString();
        
        if (notificationText != null)
            notificationText.gameObject.SetActive(false);
        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void OnSubmit()
    {
        string inputText = inputField.text;
        submitButton.isInteractable = false;
        
        StartCoroutine(SubmitScoreWithNotification(inputText));
    }
    
    private IEnumerator SubmitScoreWithNotification(string username)
    {
        yield return StartCoroutine(uploader.UploadWithCallback(Mathf.FloorToInt(dontDestroyObj.score), username, OnUploadComplete));
    }
    
    private void OnUploadComplete(bool success, string message)
    {
        if (success)
        {
            ShowNotification("Score submitted successfully!", Color.green);
        }
        else
        {
            ShowNotification("Submission failed: " + message, Color.red);
            submitButton.isInteractable = true;
        }
    }
    
    private void ShowNotification(string message, Color color)
    {
        if (notificationText != null)
        {
            notificationText.text = message;
            notificationText.color = color;
            notificationText.gameObject.SetActive(true);
            
            if (notificationPanel != null)
                notificationPanel.SetActive(true);
            
            StartCoroutine(HideNotificationAfterDelay(3f));
        }
        else
        {
            Debug.Log(message);
        }
    }
    
    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (notificationText != null)
            notificationText.gameObject.SetActive(false);
        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }
}
