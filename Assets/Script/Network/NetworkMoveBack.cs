using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class NetworkMoveBack : NetworkBehaviour
{
    private float speed = 10f;
    public GameObject prefabBullet;
    public Transform bulletSpawnPoint;
    private float interval = 0.5f;
    private float timer = 0.0f;
    public int limitBullet = 0;
    private int count = 0;
    private bool limitMode = false;
    public bool useLimitMode = false;
    private int showCount = 5;
    public TextMeshProUGUI showCountText;
    private BulletMoveType bulletMoveType = BulletMoveType.Straight;

    private BulletConfig bulletConfig;
    private OnlineGameManager ogm;

    private void Start()
    {
        Debug.Log($"NetworkMove.Start: isLocalPlayer={isLocalPlayer}, netId={netId}");
        useLimitMode = ChooseMode.mode;
        if (useLimitMode)
        {
            showCountText.text = "✖" + showCount.ToString();
        }
        if (bulletSpawnPoint == null)
        {
            Debug.LogWarning("bulletSpawnPoint is not assigned! Using player position instead.");
        }
        bulletConfig = GetComponent<BulletConfig>();
        if (bulletConfig == null)
        {
            bulletConfig = gameObject.AddComponent<BulletConfig>();
        }
        
        bulletConfig.OnBulletSettingsChanged += OnBulletSettingsChanged;
        OnBulletSettingsChanged(bulletConfig.GetCurrentSettings());
        ogm = FindAnyObjectByType<OnlineGameManager>();
        if (ogm == null)
        {
            Debug.LogWarning("OnlineGameManager not found!");
        }
    }
    
    private void OnBulletSettingsChanged(BulletSettings newSettings)
    {
        interval = newSettings.fireRate;
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        if (ogm == null || ogm.gameState != GameState.Playing) return;
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            bulletMoveType = BalletMove.NextBulletMoveType(bulletMoveType);
        }
        if (Input.GetKey(KeyCode.Space) && timer <= 0.0f && !limitMode)
        {
            var offset = bulletSpawnPoint.position - transform.position;
            BulletSettings settings = bulletConfig.GetCurrentSettings();
            CmdShoot(offset, transform.rotation, bulletMoveType, settings);
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

    [Command]
    void CmdShoot(Vector2 offset, Quaternion rotation, BulletMoveType bulletMoveType, BulletSettings settings)
    {
        var pvpNetworkManager = FindFirstObjectByType<PvPNetworkManager>();
        if (pvpNetworkManager == null) return;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y) + offset;
        GameObject bullet = Instantiate(prefabBullet, pos, rotation);
        NetworkServer.Spawn(bullet);
        pvpNetworkManager.MoveToScene(connectionToClient, bullet);
        bullet.GetComponent<Owner>().owner = netId;
        bullet.GetComponent<OnlineBulletMove>().bulletMoveType = bulletMoveType;

        // for (int i = 0; i < settings.multiShot; i++)
        // {
        //     Quaternion bulletRotation = rotation;
            
        //     // 多弾発射の場合のみ角度調整
        //     if (settings.multiShot > 1)
        //     {
        //         float totalSpread = settings.spreadAngle * (settings.multiShot - 1);
        //         float spreadOffset = -totalSpread / 2f + (settings.spreadAngle * i);
        //         bulletRotation = rotation * Quaternion.Euler(0, 0, spreadOffset);
        //     }
        //     Vector2 pos = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);
        //     GameObject bullet = Instantiate(prefabBullet, pos, bulletRotation);
            
        //     bullet.transform.localScale = settings.scale;
            
        //     NetworkServer.Spawn(bullet);
        //     pvpNetworkManager.MoveToScene(connectionToClient, bullet);
        //     bullet.GetComponent<Owner>().owner = netId;
            
        //     OnlineBulletMove bulletMove = bullet.GetComponent<OnlineBulletMove>();
        //     if (bulletMove != null)
        //     {
        //         bulletMove.bulletMoveType = bulletMoveType;
        //         bulletMove.speed = settings.speed;
        //         bulletMove.damage = settings.damage;
        //         bulletMove.piercing = settings.piercing;
        //     }
            
        //     Debug.Log("BulletNetID is " + bullet.GetComponent<Owner>().owner);
        // }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (ogm == null || ogm.gameState != GameState.Playing) return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.fixedDeltaTime;
        transform.Translate(movement);
    }
}