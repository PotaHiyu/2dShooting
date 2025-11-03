using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class NetworkMove : NetworkBehaviour
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

    private OnlineGameManager ogm;

    private void Start()
    {
        useLimitMode = ChooseMode.mode;
        if (useLimitMode && showCountText != null)
        {
            showCountText.text = "✖" + showCount.ToString();
        }
        ogm = FindAnyObjectByType<OnlineGameManager>();
    }

    void Update()
    {
        if (ogm == null || ogm.gameState != GameState.Playing) return;
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.V)) 
        {
            bulletMoveType = BalletMove.NextBulletMoveType(bulletMoveType);
        }
        if (isLocalPlayer && Input.GetKey(KeyCode.Space) && timer <= 0.0f && !limitMode)
        {
            var offset = bulletSpawnPoint.position - transform.position;
            CmdShoot(offset, transform.rotation, bulletMoveType);
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
    void CmdShoot(Vector2 offset, Quaternion rotation, BulletMoveType bulletMoveType)
    {
        var pvpNetworkManager = FindFirstObjectByType<PvPNetworkManager>();
        if (pvpNetworkManager == null) return;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y) + offset;
        GameObject bullet = Instantiate(prefabBullet, pos, rotation);
        NetworkServer.Spawn(bullet);
        pvpNetworkManager.MoveToScene(connectionToClient, bullet);
        bullet.GetComponent<Owner>().owner = netId;
        bullet.GetComponent<OnlineBulletMove>().bulletMoveType = bulletMoveType;
    }

    void FixedUpdate()
    {
        if (ogm == null || ogm.gameState != GameState.Playing) return;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.fixedDeltaTime;
        transform.Translate(movement);
    }
}