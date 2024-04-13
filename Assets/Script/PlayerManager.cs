using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int health = 1;
    private Destroy destroyScript;

    // Start is called before the first frame update
    void Start()
    {
        destroyScript = GetComponent<Destroy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health--;
            if (health == 0)
            {
                destroyScript.Destroying();
            }
        }
        else if (collision.gameObject.CompareTag("enemyBullet"))
        {
            health--;
            if (health == 0)
            {
                destroyScript.Destroying();
            }
        }
    }
}
