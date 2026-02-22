using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum OfflineBulletMoveType
{
    Normal,
    Fast,
    Disappear
}

public enum PowerUpType
{
    Speed,
    BulletSpeed,
    BulletSize,
    Damage
}

public class Move : MonoBehaviour
{
    private float speed = 10f;
    private float baseSpeed = 10f;
    public GameObject prefabBulletNormal;
    public GameObject prefabBulletFast;
    public GameObject prefabBulletDisappear;
    public Transform bulletSpawnPoint;
    private Vector2 pos;
    private float interval = 0.5f;
    private float timer = 0.0f;
    public bool debugMode = true;
    private bool isShoot = false;
    
    private OfflineBulletMoveType bulletMoveType = OfflineBulletMoveType.Normal;
    public float normalInterval = 0.5f;
    public float fastInterval = 0.8f;
    public float disappearInterval = 1f;
    
    public float powerUpIncrease = 0.5f;
    public float initialSpeed = 5f;
    public float initialBulletSpeed = 0.5f;
    public float initialBulletSize = 0.5f;
    public float initialDamage = 1f;
    public float maxSpeed = 20f;
    public float maxBulletSpeed = 1f;
    public float maxBulletSize = 5f;
    public float maxDamage = 10f;
    public bool useDebugMode = false;
    public PowerUpType debugPowerUpType = PowerUpType.Speed;
    
    private float bulletSpeedMultiplier = 1f;
    private float bulletSizeMultiplier = 1f;
    private float bulletDamageMultiplier = 1f;
    
    public AudioClip shootSound;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        baseSpeed = initialSpeed;
        speed = initialSpeed;
        bulletSpeedMultiplier = initialBulletSpeed;
        bulletSizeMultiplier = initialBulletSize;
        bulletDamageMultiplier = initialDamage;
        
        if (bulletSpawnPoint == null)
        {
            bulletSpawnPoint = transform;
        }
        
        UpdateStatusDisplay();
    }
    
    void UpdateStatusDisplay()
    {
        // 初期状態では何も表示しない
    }

    void Update()
    {
        HandleBulletTypeInput();
        HandleShooting();
        HandlePowerUpInput();

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }
    
    void HandleBulletTypeInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            bulletMoveType = OfflineBulletMoveType.Normal;
            Debug.Log("Bullet Type: Normal");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            bulletMoveType = OfflineBulletMoveType.Fast;
            Debug.Log("Bullet Type: Fast");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            bulletMoveType = OfflineBulletMoveType.Disappear;
            Debug.Log("Bullet Type: Disappear");
        }
    }
    
    void HandleShooting()
    {
        isShoot = Input.GetKey(KeyCode.Space);
        
        pos = bulletSpawnPoint.position;
        
        if (debugMode && timer <= 0.0f)
        {
            ShootBullet();
            timer = interval;
        }

        if (!debugMode && Input.GetKey(KeyCode.Space) && timer <= 0.0f)
        {
            if (bulletMoveType == OfflineBulletMoveType.Disappear)
            {
                StartCoroutine(PlayDisappearShootSound());
            }
            else
            {
                PlayShootSound();
            }
            
            ShootBullet();
            
            switch (bulletMoveType)
            {
                case OfflineBulletMoveType.Normal:
                    timer = normalInterval;
                    break;
                case OfflineBulletMoveType.Fast:
                    timer = fastInterval;
                    break;
                case OfflineBulletMoveType.Disappear:
                    timer = disappearInterval;
                    break;
            }
        }
    }
    
    void ShootBullet()
    {
        GameObject bulletPrefab = GetBulletPrefab();
        if (bulletPrefab == null)
        {
            bulletPrefab = prefabBulletNormal;
        }
        
        if (bulletPrefab == null)
        {
            Debug.LogError($"No bullet prefab assigned!");
            return;
        }
        
        GameObject bullet = Instantiate(bulletPrefab, pos, transform.rotation);
        
        bullet.transform.localScale *= bulletSizeMultiplier;
        
        BalletMove bulletMove = bullet.GetComponent<BalletMove>();
        if (bulletMove != null)
        {
            bulletMove.speed *= bulletSpeedMultiplier;
            bulletMove.offlineBulletType = (OfflineBulletType)bulletMoveType;
            
            float finalDamage = bulletDamageMultiplier;
            if (bulletMoveType == OfflineBulletMoveType.Fast)
            {
                finalDamage *= 2f;
            }
            bulletMove.damage = finalDamage;
        }
    }
    
    GameObject GetBulletPrefab()
    {
        switch (bulletMoveType)
        {
            case OfflineBulletMoveType.Normal:
                return prefabBulletNormal;
            case OfflineBulletMoveType.Fast:
                return prefabBulletFast;
            case OfflineBulletMoveType.Disappear:
                return prefabBulletDisappear;
            default:
                return prefabBulletNormal;
        }
    }
    
    void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
    
    IEnumerator PlayDisappearShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            for (int i = 0; i < 3; i++)
            {
                audioSource.PlayOneShot(shootSound);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    
    void HandlePowerUpInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ApplyPowerUp();
        }
    }
    
    public void ApplyPowerUp()
    {
        PowerUpType selectedPowerUp;
        if (useDebugMode)
        {
            selectedPowerUp = debugPowerUpType;
        }
        else
        {
            selectedPowerUp = (PowerUpType)Random.Range(0, 4);
        }
        
        Debug.Log($"PowerUp Selected: {selectedPowerUp}");
        
        switch (selectedPowerUp)
        {
            case PowerUpType.Speed:
                speed = Mathf.Min(speed + powerUpIncrease, maxSpeed);
                Debug.Log($"Power Up Applied - Speed: {speed}");
                ShowPowerUpEffect("移動速度", speed);
                break;

            case PowerUpType.BulletSpeed:
                bulletSpeedMultiplier = Mathf.Min(bulletSpeedMultiplier + powerUpIncrease, maxBulletSpeed);
                Debug.Log($"Power Up Applied - Bullet Speed: {bulletSpeedMultiplier}x");
                ShowPowerUpEffect("弾速", bulletSpeedMultiplier);
                break;

            case PowerUpType.BulletSize:
                bulletSizeMultiplier = Mathf.Min(bulletSizeMultiplier + powerUpIncrease, maxBulletSize);
                Debug.Log($"Power Up Applied - Bullet Size: {bulletSizeMultiplier}x");
                ShowPowerUpEffect("弾サイズ", bulletSizeMultiplier);
                break;

            case PowerUpType.Damage:
                bulletDamageMultiplier = Mathf.Min(bulletDamageMultiplier + powerUpIncrease, maxDamage);
                Debug.Log($"Power Up Applied - Damage: {bulletDamageMultiplier}x");
                ShowPowerUpEffect("ダメージ", bulletDamageMultiplier);
                break;
        }
    }
    
    void ShowPowerUpEffect(string powerUpName, float multiplier)
    {
        Debug.Log($"My Power Up - {powerUpName}: {multiplier}x");
        
        OfflinePowerUpTimer powerUpTimer = FindObjectOfType<OfflinePowerUpTimer>();
        if (powerUpTimer != null && powerUpTimer.valueText != null)
        {
            powerUpTimer.valueText.text = powerUpName;
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        float currentSpeed = isShoot ? speed / 2f : speed;

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * currentSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
}