using UnityEngine;
using TMPro;
using Mirror;

public class PowerUpTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float timer = 5f;
    private float interval = 5f;
    private OnlineGameManager ogm;
    private bool isCountdownActive = false;
    
    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        ogm = FindAnyObjectByType<OnlineGameManager>();
        if (ogm != null)
        {
            ogm.onGameStateChanged += OnGameStateChanged;
            if (ogm.gameState == GameState.Playing)
            {
                StartCountdown();
            }
        }
    }

    void OnDestroy()
    {
        if (ogm != null)
        {
            ogm.onGameStateChanged -= OnGameStateChanged;
        }
    }

    void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Playing)
        {
            StartCountdown();
        }
        else
        {
            StopCountdown();
        }
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
        NetworkMove[] players = FindObjectsOfType<NetworkMove>();
        foreach (NetworkMove player in players)
        {
            if (player.isLocalPlayer)
            {
                player.ApplyLocalPowerUp();
            }
        }
    }
}
