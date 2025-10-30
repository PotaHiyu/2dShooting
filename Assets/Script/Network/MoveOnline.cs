using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using TMPro;
using Mirror;

public class MoveOnline : NetworkBehaviour
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
    private int showCount = 5;
    public TextMeshProUGUI showCountText;

    private void Start()
    {
        prefabBullet = PvPNetworkManager.singleton.spawnPrefabs.Find(prefab => prefab.name == "NetworkBullet");
        useLimitMode = ChooseMode.mode;
        if (useLimitMode)
        {
            showCountText.text = "✖" + showCount.ToString();
        }
    }

    void Update()
    {
        // ローカルプレイヤーのみ入力を処理
        if (!isLocalPlayer) return;

        pos = gameObject.transform.position;
        pos.x += 1f;

        if (Input.GetKey(KeyCode.Space) && timer <= 0.0f && !limitMode)
        {
            CmdFireBullet(pos);
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

    void FixedUpdate()
    {
        // ローカルプレイヤーのみ移動処理
        if (!isLocalPlayer) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    [Command]
    void CmdFireBullet(Vector2 firePosition)
    {
        if (prefabBullet != null)
        {
            GameObject bullet = Instantiate(prefabBullet, firePosition, Quaternion.identity);
            
            // 弾がプレイヤーの弾であることを設定
            OnlineBulletMove bulletScript = bullet.GetComponent<OnlineBulletMove>();
            if (bulletScript != null)
            {
                bulletScript.isPlayer = true;
            }
            
            NetworkServer.Spawn(bullet);
        }
    }
}