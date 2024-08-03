using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Owner : NetworkBehaviour
{
    public uint owner;
    public bool isBullet = false;
    
    void Start()
    {
        CmdAssignNumber();
    }

    [Command]
    void CmdAssignNumber(){
        if (!isBullet){
            owner = netId;
        }
        Debug.Log("netId is " + owner);
    }
}
