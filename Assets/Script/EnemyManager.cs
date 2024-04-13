using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int health = 1;
    private Destroy destroyScript;
    public GameObject prefabBulletEnemy;
    private Vector2 pos;
    private Vector2 bulletPositionX;
    private float interval = 3f;
    private float timer = 0.0f;
    private float moveSpeed = 2f;
    public bool isUseGun = true;
    public bool canShoot = false;
    public bool isGoal = false;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        destroyScript = GetComponent<Destroy>();
        pos = transform.position;
    }

    void Update()
    {
        if (timer <= 0.0f && isUseGun && canShoot && !isGoal)
        {
            bulletPositionX = gameObject.transform.position;
            bulletPositionX.x -= 1f;
            Instantiate(prefabBulletEnemy, bulletPositionX, Quaternion.identity);
            timer = interval;
        }

        if (timer > 0.0f && isUseGun && canShoot && isGoal)
        {
            timer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        pos.x -= moveSpeed * Time.deltaTime;
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("bullet") && !isGoal)
        {
            Debug.Log("Hit");
            health--;
            if (health == 0)
            {
                destroyScript.Destroying();
            }
        }

        if (isGoal)
        {
            gameManager.isWin = true;
        }
    }
}
