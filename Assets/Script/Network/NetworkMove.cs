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
    public GameObject prefabBullet;
    public Transform bulletSpawnPoint;
    private float interval = 0.5f;
    private float timer = 0.0f;
    private BulletMoveType bulletMoveType = BulletMoveType.Straight;
    private OnlineGameManager ogm;
    private bool isShoot = false;

    [SyncVar] private float bulletSpeedMultiplier = 1f;
    [SyncVar] private float bulletSizeMultiplier = 1f;

    public float powerUpIncrease = 1f;

    private enum PowerUpType
    {
        Speed,
        BulletSpeed,
        BulletSize
    }

    private void Start()
    {
        ogm = FindAnyObjectByType<OnlineGameManager>();
        baseSpeed = speed;
    }

    void Update()
    {
        if (ogm == null || ogm.gameState != GameState.Playing) return;
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.V))
        {
            bulletMoveType = BalletMove.NextBulletMoveType(bulletMoveType);
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
        PowerUpType selectedPowerUp = (PowerUpType)Random.Range(0, 3);
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
                speed += powerUpIncrease;
                Debug.Log($"Power Up Applied - Speed: {powerUpIncrease}x");
                TargetShowPowerUpEffect("移動速度", powerUpIncrease);
                break;

            case PowerUpType.BulletSpeed:
                bulletSpeedMultiplier += powerUpIncrease;
                Debug.Log($"Power Up Applied - Bullet Speed: {powerUpIncrease}x");
                TargetShowPowerUpEffect("弾速", powerUpIncrease);
                break;

            case PowerUpType.BulletSize:
                bulletSizeMultiplier += powerUpIncrease;
                Debug.Log($"Power Up Applied - Bullet Size: {powerUpIncrease}x");
                TargetShowPowerUpEffect("弾の大きさ", powerUpIncrease);
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
    void CmdShoot(Vector2 offset, Quaternion rotation, BulletMoveType bulletMoveType)
    {
        var pvpNetworkManager = FindFirstObjectByType<PvPNetworkManager>();
        if (pvpNetworkManager == null) return;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y) + offset;
        GameObject bullet = Instantiate(prefabBullet, pos, rotation);

        OnlineBulletMove bulletMove = bullet.GetComponent<OnlineBulletMove>();
        if (bulletMove != null)
        {
            bulletMove.bulletMoveType = bulletMoveType;
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
