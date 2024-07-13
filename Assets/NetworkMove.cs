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

    private void Start()
    {
        useLimitMode = ChooseMode.mode;
        if (useLimitMode)
        {
            showCountText.text = "✖" + showCount.ToString();
        }
    }

    void Update()
    {
        if (isLocalPlayer && Input.GetKey(KeyCode.Space) && timer <= 0.0f && !limitMode)
        {
            CmdShoot(bulletSpawnPoint.position, transform.rotation);
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
    void CmdShoot(Vector2 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(prefabBullet, position, rotation);
        NetworkServer.Spawn(bullet);
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
