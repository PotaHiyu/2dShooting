using UnityEngine;

public class GameStateUIManager : MonoBehaviour
{
    public GameObject waitingUI;
    public GameObject countdownUI;
    public GameObject playingUI;
    public GameObject gameOverUI;

    void Start()
    {
        var ogm = FindAnyObjectByType<OnlineGameManager>();
        ogm.onGameStateChanged += ShowState;
        ShowState(ogm.gameState);
    }

    void ShowState(GameState state)
    {
        Debug.Log($"ShowState({state})");
        if (waitingUI != null) waitingUI.SetActive(state == GameState.WaitingForPlayers);
        if (countdownUI != null) countdownUI.SetActive(state == GameState.Countdown);
        if (playingUI != null) playingUI.SetActive(state == GameState.Playing);
        if (gameOverUI != null) gameOverUI.SetActive(state == GameState.GameOver);
    }
}
