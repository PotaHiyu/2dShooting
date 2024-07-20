using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SyncVar]
    private int _health;
    public int health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = Mathf.Clamp(value, 0, maxHealth);
            if (_health == 0)
            {
                CmdDestroy();
            }
        }
    }
    public int maxHealth = 100;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("TakeDamage");
        if (isClient)
        {
            CmdTakeDamage(amount);
            return;
        }
        health -= amount;
    }

    [Command]
    public void CmdTakeDamage(int amount)
    {
        health -= amount;
        Debug.Log(health);
    }

    [Command]
    private void CmdDestroy()
    {
        Destroy(gameObject);
    }
}
