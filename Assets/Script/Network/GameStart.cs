using Mirror;

public class GameStart : NetworkBehaviour
{
    private OnlineGameManager onlineGameManager
    {
        get
        {
            OnlineGameManager ogm = OnlineGameManager.instance;
            if (ogm == null)
            {
                ogm = FindAnyObjectByType<OnlineGameManager>();
            }
            return ogm;
        }
    }

    [ClientRpc]
    public void RpcStartCountdown(int time)
    {
        onlineGameManager.StartCountdown(time);
    }

    [ClientRpc]
    public void RpcStartPlaying()
    {
        onlineGameManager.StartPlaying();
    }
}
