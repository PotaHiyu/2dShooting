using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SyncVar]
    public int health {get; private set;}
    public int maxHealth = 100;

    void Start()
    {
        health = maxHealth;
    }

    [Command]
    void CmdTakeDamage(int amount)
    {
        health = Mathf.Min(Mathf.Max(health - amount, 0), maxHealth);
    }
}
