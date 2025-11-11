using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OnlineBulletMove : NetworkBehaviour
{
    public bool isPlayer;
    public float speed;
    private float baseSpeed;
    [SyncVar] public float speedMultiplier = 1f;
    // public int damage;
    // public bool piercing = false;
    [SyncVar] private float rand;
    private Destroy destroyScript;
    [SyncVar]
    public BulletMoveType bulletMoveType = BulletMoveType.Straight;

    void Start()
    {
        destroyScript = GetComponent<Destroy>();
        baseSpeed = speed;
        if (isServer)
        {
            rand = Random.Range(-0.01f, 0.01f);
        }
    }

    void FixedUpdate()
    {
        if (!isServer) return;
        switch (bulletMoveType)
        {
            case BulletMoveType.Straight:
                Straight();
                break;
            case BulletMoveType.Curve:
                Curve();
                break;
        }
    }

    void Curve()
    {
        Debug.Log("Curve");
    }

    void Straight()
    {
        Vector3  movement = new Vector3(0, rand, 0);
        Vector2 pos = transform.position;
        pos.x += speed * Time.fixedDeltaTime * ((transform.rotation.eulerAngles.y + 90) % 360 < 180 ? 1 : -1);
        transform.position = pos;
        transform.position += movement;
    }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isServer) return;
            if (collision.gameObject.CompareTag("MoveWall"))
            {
                NetworkServer.Destroy(gameObject);
                return;
            }
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