using UnityEngine;
using TMPro;

public class OfflinePowerUpTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI valueText;
    private float timer = 5f;
    public float interval = 5f;
    private bool isCountdownActive = false;
    
    void Awake()
    {
        if (timerText == null)
        {
            timerText = GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        if (valueText != null)
        {
            valueText.text = "";
        }
        StartCountdown();
    }

    void StartCountdown()
    {
        isCountdownActive = true;
        timer = interval;
    }

    void StopCountdown()
    {
        isCountdownActive = false;
    }

    void Update()
    {
        if (!isCountdownActive) return;
        
        timer -= Time.deltaTime;
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(timer).ToString("F0");
        }
        
        if (timer <= 0f)
        {
            NotifyPowerUp();
            timer = interval;
        }
    }
    
    void NotifyPowerUp()
    {
        if (valueText != null)
        {
            valueText.text = "";
        }
        
        Move[] players = FindObjectsOfType<Move>();
        foreach (Move player in players)
        {
            player.ApplyPowerUp();
        }
    }
}