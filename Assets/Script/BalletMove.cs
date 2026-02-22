using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletMoveType
{
    Straight,
    Curve
}

public enum OfflineBulletType
{
    Normal,
    Fast,
    Disappear
}

public class BalletMove : MonoBehaviour
{
    public bool isPlayer;
    public float speed;
    private float baseSpeed;
    private float rand;
    private Destroy destroyScript;
    public BulletMoveType bulletMoveType = BulletMoveType.Straight;
    public OfflineBulletType offlineBulletType = OfflineBulletType.Normal;
    public float damage = 1f;
    
    private SpriteRenderer spriteRenderer;
    private float disappearTimer = 0f;
    private float disappearStartTime = 0.3f;
    private float disappearDuration = 0.1f;
    private float invisibleDuration = 3f;
    private float fastSpeedMultiplier = 1.5f;
    private bool isFast = false;

    void Start()
    {
        destroyScript = GetComponent<Destroy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseSpeed = speed;
        
        switch (offlineBulletType)
        {
            case OfflineBulletType.Normal:
                rand = Random.Range(-0.03f, 0.03f);
                break;
            case OfflineBulletType.Fast:
                rand = Random.Range(-0.01f, 0.01f);
                speed = baseSpeed * fastSpeedMultiplier;
                isFast = true;
                break;
            case OfflineBulletType.Disappear:
                rand = 0f;
                speed = baseSpeed / 4;
                break;
        }
    }

    void Update()
    {
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
    
    void HandleDisappearEffect()
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
    
    void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
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
        Vector3 movement = new Vector3(0, rand, 0);
        Vector2 pos = transform.position;
        pos.x += speed * Time.fixedDeltaTime * ((transform.rotation.eulerAngles.y + 90) % 360 < 180 ? 1 : -1);
        transform.position = pos;
        transform.position += movement;
    }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isFast)
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
            else
            {
                if (collision.gameObject.CompareTag("Player") && isPlayer == false)
                {
                    destroyScript.Destroying();
                }
            }
            
        }
}