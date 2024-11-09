using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalletMove : MonoBehaviour
{
    public bool isPlayer;
    public float speed;
    private float rand;
    private Destroy destroyScript;

    void Start()
    {
        destroyScript = GetComponent<Destroy>();
    }


    void FixedUpdate()
    {
        rand = Random.Range(-0.01f, 0.02f);
        Vector3  movement = new Vector3(0, rand, 0);
        Vector2 pos = transform.position;
        pos.x += speed * Time.fixedDeltaTime * ((transform.rotation.eulerAngles.y + 90) % 360 < 180 ? 1 : -1);
        transform.position = pos;
        transform.position += movement;
    }

        void OnTriggerEnter2D(Collider2D collision)
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