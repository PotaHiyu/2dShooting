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
        Debug.Log($"Damage.OnTriggerEnter2D isServer? {isServer}");
        Health healthScript = other.GetComponent<Health>();
        Debug.Log(healthScript);
        if (healthScript == null) return;
        Owner otherOwner = other.GetComponent<Owner>();
        uint otherId;
        if (otherOwner == null)
        {
            otherId = other.GetComponent<NetworkIdentity>().netId;
        }
        else
        {
            otherId = otherOwner.owner;
        }
        Owner myOwner = GetComponent<Owner>();
        if (otherId == myOwner.owner) return;
        healthScript.TakeDamage(damage);
        if (DestroyOnHit)
        {
            Destroy(gameObject);
        }
    }
}
