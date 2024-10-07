using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Death : NetworkBehaviour
{
    public GameObject deathVFX;
    public float deathTime = 2f;
    
    void Start()
    {
        Health health = GetComponent<Health>();
        if (health == null) return;
        health.onPlayerDied += OnPlayerDied;
    }

    public void OnPlayerDied()
    {
        if (isClient) StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {   
        if (deathVFX) SpawnVFX();
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;
        yield return new WaitForSeconds(deathTime);
    }


    void SpawnVFX()
    {
        Instantiate(deathVFX, transform.position, Quaternion.identity);
    }
}
