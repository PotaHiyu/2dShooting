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
        Debug.Log($"TakeDamage {isClient} {isServer} {isLocalPlayer} {authority}");
        health = Mathf.Clamp(health - amount, 0, maxHealth);
    }

    void HealthHandler(int oldHealth, int newHealth)
    {
        Debug.Log($"HealthHandler {isClient} {isServer} {isLocalPlayer} {authority}");
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
        if (deathVFX && isClient) CmdSpawnVFX();
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;
    }

    IEnumerator DestroyAction()
    {
        Debug.Log($"DestroyAction {isClient} {isServer} {isLocalPlayer} {authority}");
        if (isLocalPlayer)
        {
            yield return new WaitForSeconds(deathTime);
            // player will disconnect
            Debug.Log("Disconnecting!");
            connectionToServer.Disconnect();
            // Destroy(gameObject);
        }
    }

    // [Command]
    void CmdSpawnVFX()
    {
        // var pvpNetworkManager = FindFirstObjectByType<PvPNetworkManager>();
        // Debug.Log("Spawning vfx");
        // Debug.Log(pvpNetworkManager);
        // if (pvpNetworkManager == null) return;

        GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        // NetworkServer.Spawn(vfx);
        // pvpNetworkManager.MoveToScene(connectionToClient, vfx);
    }
}
