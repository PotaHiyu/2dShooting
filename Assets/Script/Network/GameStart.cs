using Mirror;

public class GameStart : NetworkBehaviour {
    [ClientRpc]
    public void RpcStartCountdown(int time)
    {
        {
            var ogm = FindAnyObjectByType<OnlineGameManager>();
            if (ogm != null) ogm.StartCountdown(time);
        }
    }

    [ClientRpc]
    public void RpcStartPlaying()
    {
        var ogm = FindAnyObjectByType<OnlineGameManager>();
        if (ogm != null) ogm.StartPlaying();
    }
}