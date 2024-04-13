using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalletMove : MonoBehaviour
{
    private Vector2 pos;
    public bool isReverse;
    public bool isPlayer;
    public float speed;
    private Destroy destroyScript;

    void Start()
    {
        pos = gameObject.transform.position;
        destroyScript = GetComponent<Destroy>();
    }


    void FixedUpdate()
    {
        if (isReverse)
        {
            pos.x -= speed * Time.fixedDeltaTime;
        }
        else
        {
            pos.x += speed * Time.fixedDeltaTime;
        }
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPlayer == false)
        {
            destroyScript.Destroying();
        }
        else if (collision.gameObject.CompareTag("Enemy") && isPlayer == true)
        {
            destroyScript.Destroying();
        }
    }
}
