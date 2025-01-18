using UnityEngine;
using Mirror;
using System.Collections;

public class AutoConnect : MonoBehaviour
{
    NetworkManager manager;

    void Start()
    {
        manager = GetComponent<NetworkManager>();
        manager.networkAddress = "127.0.0.1";

        if (Application.isBatchMode)
            return;
        
        manager.StartClient();
        StartCoroutine(MakePlayer());
    }

    IEnumerator MakePlayer()
    {
        while(!NetworkClient.isConnected)
        {
            yield return null;
        }
        NetworkClient.Ready();
        if (NetworkClient.localPlayer == null)
            NetworkClient.AddPlayer();

        //相手がいるかいないかのチェック
    }
}