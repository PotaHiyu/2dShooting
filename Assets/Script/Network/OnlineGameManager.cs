using Mirror;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    WaitingForPlayers,
    Countdown,
    Playing,
    GameOver
}

public class OnlineGameManager : NetworkBehaviour
{
    // public static OnlineGameManager instance;
    public UnityAction<GameState> onGameStateChanged;
    protected GameState gameState_ = GameState.WaitingForPlayers;
    public GameState gameState
    {
        get
        {
            return gameState_;
        }
        protected set
        {
            gameState_ = value;
            Debug.Log($"Set gameState to {value}");
            onGameStateChanged?.Invoke(gameState_);
        }
    }
    public int countdownTime = 3;

    // private void Awake()
    // {
    //     if (instance != null)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }
    //     instance = this;
    // }

    public void StartCountdown(int time)
    {
        Debug.Log("OnlineGameManager.StartCountdown()");
        countdownTime = time;
        gameState = GameState.Countdown;
    }

    public void StartPlaying()
    {
        Debug.Log("OnlineGameManager.StartPlaying()");
        gameState = GameState.Playing;
    }
}
