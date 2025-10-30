using Mirror;
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
            onGameStateChanged?.Invoke(gameState_);
        }
    }

    public int countdownTime = 3;

    public void StartCountdown(int time)
    {
        countdownTime = time;
        gameState = GameState.Countdown;
    }

    public void StartPlaying()
    {
        gameState = GameState.Playing;
    }
}