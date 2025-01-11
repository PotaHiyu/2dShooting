using UnityEngine;
using Mirror;

public class AutoConnect : MonoBehaviour
{
    void Start()
    {
        NetworkManager.singleton.networkAddress = "127.0.0.1"; // TODO: Make a way to look up the server address.

        // Don't automatically connect if we're the server or already connected.
        if (Application.isBatchMode || NetworkClient.isConnected)
            return;

        NetworkManager.singleton.StartClient();
    }
}
