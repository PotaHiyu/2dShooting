using UnityEngine;
using Mirror;

public class AutoConnect : MonoBehaviour
{
    NetworkManager manager;

    void Start()
    {
        manager = GetComponent<NetworkManager>();
        manager.networkAddress = "127.0.0.1";

        if (Application.isBatchMode || NetworkClient.isConnected)
            return;
        
        manager.StartClient();
    }
}