using UnityEngine;
using Mirror;
using System.Collections;

public class AutoConnect : MonoBehaviour
{
    NetworkManager manager;

    void Start()
    {
        manager = GetComponent<NetworkManager>();
        manager.networkAddress = "127.0.0.1"; // TODO: Make a way to look up the server address.
        manager.StartClient();
        StartCoroutine(MakePlayer());
    }

    IEnumerator MakePlayer()
    {
        Debug.Log("Waiting for connection...");
        while (!NetworkClient.isConnected)
        {
            yield return null;
        }

        Debug.Log("Connected!");
        NetworkClient.Ready();
        if (NetworkClient.localPlayer == null)
            NetworkClient.AddPlayer();

        // TODO: Wait until the opponent is ready.
        // TODO: Count down to start the game.

        // TODO: After, need to pass data to the end scene and destroy the network manager.
        // TODO: It seems like the network client isn't disconnecting after the death animation.
    }
}
