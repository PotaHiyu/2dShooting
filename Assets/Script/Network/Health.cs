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
    public GameObject deathVFX;
    public float deathTime = 2f;
    public UnityAction onPlayerDied;

    void Start()
    {
        health = maxHealth;
        onPlayerDied += OnPlayerDied;
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        if (health == 0)
        {
            StartCoroutine(DestroyAction());
        }
    }

    void HealthHandler (int oldHealth, int newHealth)
    {
        if (health == 0 && isClient)
        {
            if (isLocalPlayer)
            {
                StartCoroutine(DestroyAction());
            }
            if (onPlayerDied != null) onPlayerDied.Invoke();
        }
    }

    public void OnPlayerDied()
    {
        if (deathVFX && isClient) SpawnVFX();
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;
    }

    IEnumerator DestroyAction()
    {
        if (isLocalPlayer)
        {
            yield return new WaitForSeconds(deathTime);
            connectionToServer.Disconnect();
        }
        
    }

    void SpawnVFX()
    {
        Instantiate(deathVFX, transform.position, Quaternion.identity);
    }
}
