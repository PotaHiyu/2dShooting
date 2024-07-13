using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalletMove : MonoBehaviour
{
    public bool isReverse;
    public bool isPlayer;
    public float speed;
    private Destroy destroyScript;

    void Start()
    {
        destroyScript = GetComponent<Destroy>();
    }


    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x += speed * Time.fixedDeltaTime * ((transform.rotation.eulerAngles.y + 90) % 360 < 180 ? 1 : -1);
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
