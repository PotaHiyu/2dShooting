using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletMoveType
{
    Straight,
    Curve
}

public class BalletMove : MonoBehaviour
{
    public bool isPlayer;
    public float speed;
    public int damage = 1;
    public bool piercing = false;
    private float rand;
    private Destroy destroyScript;
    public BulletMoveType bulletMoveType = BulletMoveType.Straight;

    void Start()
    {
        destroyScript = GetComponent<Destroy>();
    }

    void FixedUpdate()
    {
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

    public static BulletMoveType NextBulletMoveType(BulletMoveType bulletMoveType)
    {
        switch (bulletMoveType)
        {
            case BulletMoveType.Straight:
                return BulletMoveType.Curve;
            case BulletMoveType.Curve:
                return BulletMoveType.Straight;
            default:
                return BulletMoveType.Straight;
        }
    }

    void Curve()
    {
        Debug.Log("Curve");
    }

    void Straight()
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
            PlayerManager playerManager = collision.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.health -= damage;
            }
            
            if (!piercing)
            {
                destroyScript.Destroying();
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
                destroyScript.Destroying();
            }
        }
    }
}