using UnityEngine;
using Mirror;
using Mirror.SimpleWeb;

public class AutoConnect : MonoBehaviour
{
    NetworkManager manager;
    public ushort clientWssPort = 443;

    void Start()
    {
        manager = GetComponent<NetworkManager>();
        var ws = GetComponent<SimpleWebTransport>();
        if (ws != null && clientWssPort > 0)
        {
            ws.port = clientWssPort;
        }

        if (Application.isBatchMode || NetworkClient.isConnected)
            return;
        
        manager.StartClient();
    }
}