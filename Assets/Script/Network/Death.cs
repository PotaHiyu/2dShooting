using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Death : NetworkBehaviour
{
    public GameObject deathVFX;
    public float deathTime = 2f;
    public bool isPlayer = false;
    
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
        if (isPlayer) {
            Renderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.enabled = false;
        }
        else if (!isPlayer)
        {
            gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(deathTime);
    }


    void SpawnVFX()
    {
        Instantiate(deathVFX, transform.position, Quaternion.identity);
    }
}
