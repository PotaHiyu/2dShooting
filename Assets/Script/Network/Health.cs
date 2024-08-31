using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SyncVar]
    public int health;
    public int maxHealth = 100;
    public GameObject deathVFX;
    public float deathTime = 2f;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        if (health == 0)
        {
            StartCoroutine(DestroyAction());
        }
    }

    IEnumerator DestroyAction()
    {
        // player died callback
        if (deathVFX) CmdSpawnVFX();
        yield return new WaitForSeconds(deathTime);
        // player will disconnect
        Destroy(gameObject);
    }

    [Command]
    void CmdSpawnVFX()
    {
        var pvpNetworkManager = FindFirstObjectByType<PvPNetworkManager>();
        if (pvpNetworkManager == null) return;

        GameObject vfx = Instantiate(deathVFX, transform);
        NetworkServer.Spawn(vfx);
        pvpNetworkManager.MoveToScene(connectionToClient, vfx);
    }
}
