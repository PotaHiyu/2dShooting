using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using TMPro;
using Mirror;

public class NetworkMove : NetworkBehaviour
{
    
    [SyncVar] public float speed = 5f;
    private float baseSpeed = 5f;
    public GameObject prefabBulletNormal;
    public GameObject prefabBulletFast;
    public GameObject prefabBulletDisappear;
    public Transform bulletSpawnPoint;
    private float interval = 0.5f;
    private float timer = 0.0f;
    private NetworkBulletMoveType bulletMoveType = NetworkBulletMoveType.Normal;
    private OnlineGameManager ogm;
    private bool isShoot = false;

    [SyncVar] private float bulletSpeedMultiplier = 1f;
    [SyncVar] private float bulletSizeMultiplier = 1f;

    [Header("Power Up Settings")]
    public float powerUpIncrease = 5f;
    public float initialSpeed = 1f;
    public float initialBulletSpeed = 1f;
    public float initialBulletSize = 1f;
    [Header("Max Limits")]
    public float maxSpeed = 20f;
    public float maxBulletSpeed = 15f;
    public float maxBulletSize = 5f;
    [Header("Debug Settings")]
    public bool useDebugMode = false;
    public PowerUpType debugPowerUpType = PowerUpType.Speed;

    public enum NetworkBulletMoveType
    {
        Normal,
        Fast,
        Disappear
    }

    public enum PowerUpType
    {
        Speed,
        BulletSpeed,
        BulletSize
    }

    private void Start()
    {
        ogm = FindAnyObjectByType<OnlineGameManager>();
        
        if (isServer)
        {
            speed = initialSpeed;
            baseSpeed = initialSpeed;
            bulletSpeedMultiplier = initialBulletSpeed;
            bulletSizeMultiplier = initialBulletSize;
        }
    }

    void Update()
    {
        if (ogm == null || ogm.gameState != GameState.Playing) return;
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                bulletMoveType = NetworkBulletMoveType.Normal;
                Debug.Log("Bullet Type: Normal");
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                bulletMoveType = NetworkBulletMoveType.Fast;
                Debug.Log("Bullet Type: Fast");
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                bulletMoveType = NetworkBulletMoveType.Disappear;
                Debug.Log("Bullet Type: Disappear");
            }
        }
        if (isLocalPlayer)
        {
            isShoot = Input.GetKey(KeyCode.Space);
        }
        if (isLocalPlayer && Input.GetKey(KeyCode.Space) && timer <= 0.0f)
        {
            var offset = bulletSpawnPoint.position - transform.position;
            CmdShoot(offset, transform.rotation, bulletMoveType);
            timer = interval;
        }

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }
    public void ApplyLocalPowerUp()
    {
        PowerUpType selectedPowerUp;
        if (useDebugMode)
        {
            selectedPowerUp = debugPowerUpType;
        }
        else
        {
            selectedPowerUp = (PowerUpType)Random.Range(0, 3);
        }
        Debug.Log($"[{netId}] Local PowerUp Selected: {selectedPowerUp}");
        CmdApplyPowerUp(selectedPowerUp);
    }
    
    [Command]
    void CmdApplyPowerUp(PowerUpType powerUpType)
    {
        Debug.Log($"[Server] Applying PowerUp for {netId}: {powerUpType}");
        Debug.Log($"[Server] Before - speed: {speed}, bulletSpeed: {bulletSpeedMultiplier}, bulletSize: {bulletSizeMultiplier}");
        switch (powerUpType)
        {
            case PowerUpType.Speed:
                speed = Mathf.Min(speed + powerUpIncrease, maxSpeed);
                Debug.Log($"Power Up Applied - Speed: {speed}x");
                TargetShowPowerUpEffect("移動速度", speed);
                break;

            case PowerUpType.BulletSpeed:
                bulletSpeedMultiplier = Mathf.Min(bulletSpeedMultiplier + powerUpIncrease, maxBulletSpeed);
                Debug.Log($"Power Up Applied - Bullet Speed: {bulletSpeedMultiplier}x");
                TargetShowPowerUpEffect("弾速", bulletSpeedMultiplier);
                break;

            case PowerUpType.BulletSize:
                bulletSizeMultiplier = Mathf.Min(bulletSizeMultiplier + powerUpIncrease, maxBulletSize);
                Debug.Log($"Power Up Applied - Bullet Size: {bulletSizeMultiplier}x");
                TargetShowPowerUpEffect("弾の大きさ", bulletSizeMultiplier);
                break;
        }
        Debug.Log($"[Server] After - speed: {speed}, bulletSpeed: {bulletSpeedMultiplier}, bulletSize: {bulletSizeMultiplier}");
    }

    [TargetRpc]
    void TargetShowPowerUpEffect(string powerUpName, float multiplier)
    {
        Debug.Log($"[Client] My Power Up - {powerUpName}: {multiplier}x");
    }

    [Command]
    void CmdShoot(Vector2 offset, Quaternion rotation, NetworkBulletMoveType bulletMoveType)
    {
        var pvpNetworkManager = FindFirstObjectByType<PvPNetworkManager>();
        if (pvpNetworkManager == null) return;

        GameObject bulletPrefab = null;
        switch (bulletMoveType)
        {
            case NetworkBulletMoveType.Normal:
                bulletPrefab = prefabBulletNormal;
                break;
            case NetworkBulletMoveType.Fast:
                bulletPrefab = prefabBulletFast;
                break;
            case NetworkBulletMoveType.Disappear:
                bulletPrefab = prefabBulletDisappear;
                break;
        }

        if (bulletPrefab == null)
        {
            Debug.LogError($"Bullet prefab for {bulletMoveType} is not assigned!");
            return;
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y) + offset;
        GameObject bullet = Instantiate(bulletPrefab, pos, rotation);

        OnlineBulletMove bulletMove = bullet.GetComponent<OnlineBulletMove>();
        if (bulletMove != null)
        {
            bulletMove.speedMultiplier = bulletSpeedMultiplier;
        }

        bullet.transform.localScale *= bulletSizeMultiplier;

        NetworkServer.Spawn(bullet);
        pvpNetworkManager.MoveToScene(connectionToClient, bullet);
        bullet.GetComponent<Owner>().owner = netId;
    }

    void FixedUpdate()
    {
        if (ogm == null || ogm.gameState != GameState.Playing) return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float currentSpeed = isShoot ? speed / 2f : speed;

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * currentSpeed * Time.fixedDeltaTime;
        transform.Translate(movement);
    }
}
