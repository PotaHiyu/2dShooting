using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Damage : NetworkBehaviour
{
    public int damage;
    public bool DestroyOnHit;

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("a");
        Health healthScript = other.GetComponent<Health>();
        Debug.Log(healthScript);
        if (healthScript == null) return;
        healthScript.TakeDamage(damage);
        if (DestroyOnHit)
        {
            Destroy(gameObject);
        }
    }
}
