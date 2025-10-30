using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OnlineBulletMove : NetworkBehaviour
{
    public bool isPlayer;
    public float speed;
    public int damage = 1;
    public bool piercing = false;
    [SyncVar] private float rand;
    private Destroy destroyScript;
    [SyncVar]
    public BulletMoveType bulletMoveType = BulletMoveType.Straight;

    void Start()
    {
        destroyScript = GetComponent<Destroy>();
        if (isServer)
        {
            rand = Random.Range(-0.01f, 0.02f);
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
        
        // MoveWallは貫通弾でも必ず止める（優先判定）
        if (collision.gameObject.CompareTag("MoveWall"))
        {
            Debug.Log($"弾がMoveWallに衝突: {gameObject.name}");
            NetworkServer.Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.name.Contains("Wall") || collision.gameObject.name.Contains("wall"))
        {
            Debug.Log($"弾がWall系オブジェクトに衝突: {collision.gameObject.name}");
            NetworkServer.Destroy(gameObject);
            return;
        }
        
        if (collision.gameObject.CompareTag("Player") && isPlayer == false)
        {
            // オンラインプレイヤーの体力管理（Health コンポーネント使用）
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            
            if (!piercing)
            {
                NetworkServer.Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy") && isPlayer == true)
        {
            EnemyManager enemyManager = collision.GetComponent<EnemyManager>();
            if (enemyManager != null)
            {
                enemyManager.health -= damage;
            }
            
            if (!piercing)
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}