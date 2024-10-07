using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    [SyncVar(hook = nameof(HealthHandler))]
    public int health;
    public int maxHealth = 100;
    public UnityAction onPlayerDied;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
    }

    void HealthHandler (int oldHealth, int newHealth)
    {
        if (health == 0 && isClient)
        {
            if (onPlayerDied != null) onPlayerDied.Invoke();
        }
    }
}
