using UnityEngine;
using TMPro;
using Michsky.MUIP;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private DontDestroyOnLoad dontDestroyObj;
    public TMP_InputField inputField;
    public ButtonManager submitButton;
    private LeaderboardUploader uploader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // dontDestroyObj = GameObject.Find("DontDestroyOnLoad").GetComponent<DontDestroyOnLoad>();
        // uploader = gameObject.GetComponent<LeaderboardUploader>();
        // scoreText.text = "Score: " + Mathf.FloorToInt(dontDestroyObj.score).ToString();
    }

    public void OnSubmit()
    {
        // string inputText = inputField.text;
        // uploader.SaveData(Mathf.FloorToInt(dontDestroyObj.score), inputText);

        // submitButton.isInteractable = false;
    }
}
