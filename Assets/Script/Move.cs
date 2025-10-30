using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Move : MonoBehaviour
{
    private float speed = 10f;
    public GameObject prefabBullet;
    private Vector2 pos;
    private float interval = 0.5f;
    private float timer = 0.0f;
    public int limitBullet = 0;
    private int count = 0;
    private bool limitMode = false;
    public bool useLimitMode = false;
    public bool debugMode = true;
    private int showCount = 5;
    public TextMeshProUGUI showCountText;
    
    private BulletConfig bulletConfig;
    private void Start()
    {
        useLimitMode = ChooseMode.mode;
        if (useLimitMode)
        {
            showCountText.text = "✖" + showCount.ToString();
        }
        
        bulletConfig = GetComponent<BulletConfig>();
        if (bulletConfig == null)
        {
            bulletConfig = gameObject.AddComponent<BulletConfig>();
        }
        
        bulletConfig.OnBulletSettingsChanged += OnBulletSettingsChanged;
        OnBulletSettingsChanged(bulletConfig.GetCurrentSettings());
    }
    
    private void OnBulletSettingsChanged(BulletSettings newSettings)
    {
        interval = newSettings.fireRate;
    }

    void Update()
    {
        pos = gameObject.transform.position;
        pos.x += 1f;

        if (debugMode && timer <= 0.0f)
        {
            FireBullet(pos);
            timer = interval;
        }

        if (!debugMode && Input.GetKey(KeyCode.Space) && timer <= 0.0f && !limitMode)
        {
            FireBullet(pos);
            timer = interval;
            if (showCount > 0 && useLimitMode)
            {
                count += 1;
                showCount -= 1;
                showCountText.text = "✖" + showCount.ToString();
            }
        }

        if (count == limitBullet && useLimitMode)
        {
            limitMode = true;
        }

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }

    private void FireBullet(Vector2 position)
    {
        BulletSettings settings = bulletConfig.GetCurrentSettings();
        
        for (int i = 0; i < settings.multiShot; i++)
        {
            float angle = 0f;
            if (settings.multiShot > 1)
            {
                float totalSpread = settings.spreadAngle * (settings.multiShot - 1);
                angle = -totalSpread / 2f + (settings.spreadAngle * i);
            }
            
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(prefabBullet, position, rotation);
            
            bullet.transform.localScale = settings.scale;
            
            BalletMove bulletMove = bullet.GetComponent<BalletMove>();
            if (bulletMove != null)
            {
                bulletMove.speed = settings.speed;
                bulletMove.damage = settings.damage;
                bulletMove.piercing = settings.piercing;
            }
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}