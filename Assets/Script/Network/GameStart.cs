using Mirror;
using UnityEngine;

public class GameStart : NetworkBehaviour
{
    [ClientRpc]
    public void RpcStartCountdown(int time)
    {
        Debug.Log($"[CLIENT] GameStart.RpcStartCountdown({time})");
        StartCoroutine(WaitForOnlineGameManager(time, true));
    }
    
    private System.Collections.IEnumerator WaitForOnlineGameManager(int time, bool isCountdown)
    {
        OnlineGameManager ogm = null;
        float timeout = 3.0f;
        
        while (ogm == null && timeout > 0)
        {
            ogm = FindAnyObjectByType<OnlineGameManager>();
            if (ogm == null)
            {
                Debug.Log("[CLIENT] Waiting for OnlineGameManager...");
                yield return new WaitForSeconds(0.1f);
                timeout -= 0.1f;
            }
        }
        
        if (ogm != null)
        {
            Debug.Log($"[CLIENT] OnlineGameManager found after waiting");
            if (isCountdown)
                ogm.StartCountdown(time);
            else
                ogm.StartPlaying();
        }
        else
        {
            Debug.LogError($"[CLIENT] OnlineGameManager not found after {3.0f} seconds timeout!");
        }
    }

    [ClientRpc]
    public void RpcStartPlaying()
    {
        Debug.Log("[CLIENT] GameStart.RpcStartPlaying()");
        StartCoroutine(WaitForOnlineGameManager(0, false));
    }
}
