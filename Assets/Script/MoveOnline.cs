using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using TMPro;
using Mirror;

public class MoveOnline : MonoBehaviour
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
        pos = gameObject.transform.position;
        pos.x += 1f;

        if (Input.GetKey(KeyCode.Space) && timer <= 0.0f && !limitMode)
        {
            GameObject bullet = Instantiate(prefabBullet, pos, Quaternion.identity);
            NetworkServer.Spawn(bullet);
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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}