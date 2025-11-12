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
    public NetworkMove.NetworkBulletMoveType bulletMoveType = NetworkMove.NetworkBulletMoveType.Normal;

    private SpriteRenderer spriteRenderer;
    private float disappearTimer = 0f;
    private float disappearStartTime = 0.3f; // 発射してから消え始めるまでの時間
    private float disappearDuration = 0.1f; // 完全に透明になるまでの時間
    private float invisibleDuration = 3f; // 透明時間
    private float reappearDuration = 0f; // 完全に見えなくなってから見えるようになるまでの時間

    private float fastSpeed = 1.5f;

    void Start()
    {
        destroyScript = GetComponent<Destroy>();
        baseSpeed = speed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isServer)
        {
            switch (bulletMoveType)
            {
                case NetworkMove.NetworkBulletMoveType.Normal:
                    rand = Random.Range(-0.01f, 0.01f);
                    speed = baseSpeed * speedMultiplier;
                    break;
                case NetworkMove.NetworkBulletMoveType.Fast:
                    speed = baseSpeed * fastSpeed;
                    rand = Random.Range(-0.03f, 0.03f);
                    break;
                case NetworkMove.NetworkBulletMoveType.Disappear:
                    rand = 0f;
                    speed = baseSpeed / 4;
                    break;
            }
        }
    }

    void Update()
    {
        if (bulletMoveType == NetworkMove.NetworkBulletMoveType.Disappear && spriteRenderer != null)
        {
            disappearTimer += Time.deltaTime;

            if (disappearTimer < disappearStartTime)
            {
                SetAlpha(1f);
            }
            else if (disappearTimer < disappearStartTime + disappearDuration)
            {
                float progress = (disappearTimer - disappearStartTime) / disappearDuration;
                SetAlpha(1f - progress);
            }
            else if (disappearTimer < disappearStartTime + disappearDuration + invisibleDuration)
            {
                SetAlpha(0f);
            }
            else
            {
                SetAlpha(1f);
                disappearTimer = 0f;
            }
        }
    }

    void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    void FixedUpdate() {
        if (!isServer) return;
        MoveBullet();
    }

    void MoveBullet()
    {
        Vector3 movement = new Vector3(0, rand, 0);
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