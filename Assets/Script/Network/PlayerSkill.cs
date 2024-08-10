using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSkill : NetworkBehaviour
{
    public GameObject prefab;
    public Transform skillPosition;
    [SyncVar]
    private float count = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CmdSpawnSkill(skillPosition.position);
        }
    }

    [Command]
    void CmdSpawnSkill(Vector2 pos)
    {
        if (count >= 1)
        {
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            NetworkServer.Spawn(go);
            count -= 1;
            go.GetComponent<Owner>().owner = netId;
        }
    }
}
