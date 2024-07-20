using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SyncVar]
    public int health;
    //[SyncVar]
    //[SerializeField]
    //private int _health;
    // public int health
    // {
    //     get
    //     {
    //         return _health;
    //     }
    //     private set
    //     {
    //         _health = Mathf.Clamp(value, 0, maxHealth);
    //         if (_health == 0)
    //         {
    //             Destroy(gameObject);
    //         }
    //     }
    // }
    public int maxHealth = 100;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("TakeDamage");
        Debug.Log($"before [Server? {isServer}] Health is now {health}");
        // if (isClient)
        // {
        //     Debug.Log("returning because isClient was true");
        //     return;
        // }
        health -= amount;
        Debug.Log($"after [Server? {isServer}] Health is now {health}");
    }
}
