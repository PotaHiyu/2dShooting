using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SyncVar]
    public int health;
    public int maxHealth = 100;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}
