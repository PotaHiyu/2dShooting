using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSkill : NetworkBehaviour
{
    public GameObject prefab;
    public Transform skillPosition;
    private int shootType;
    [SyncVar]
    private float count = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CmdSpawnSkill(skillPosition.position);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            CmdChangeShootType();
        }
    }

    [Command]
    void CmdSpawnSkill(Vector2 pos)
    {
        if (count >= 1)
        {
            var pvpNetworkManager = FindFirstObjectByType<PvPNetworkManager>();
            if (pvpNetworkManager == null) return;

            GameObject go = Instantiate(prefab, new Vector3(pos.x, pos.y, -1), Quaternion.identity);
            NetworkServer.Spawn(go);
            pvpNetworkManager.MoveToScene(connectionToClient, go);
            go.GetComponent<Owner>().owner = netId;
            count -= 1;
        }
    }

    [Command]
    void CmdChangeShootType()
    {
        if (shootType == 0)
        {

        }
        else if (shootType == 1)
        {

        }
        else
        {

        }
    }
}
