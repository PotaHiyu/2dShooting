using Mirror;
using UnityEngine;

public class GameStart : NetworkBehaviour
{
    [ClientRpc]
    public void RpcStartCountdown(int time)
    {
        Debug.Log($"GameStart.RpcStartCountdown({time})");
        var ogm = FindAnyObjectByType<OnlineGameManager>();
        if (ogm != null) ogm.StartCountdown(time);
    }

    [ClientRpc]
    public void RpcStartPlaying()
    {
        Debug.Log("GameStart.RpcStartPlaying()");
        var ogm = FindAnyObjectByType<OnlineGameManager>();
        if (ogm != null) ogm.StartPlaying();
    }
}
