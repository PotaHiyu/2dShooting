using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Dash,
    Gun,
    Bomb
}

public class EnemyMove : MonoBehaviour
{
    public EnemyType enemyType;
    public GameObject prefabBulletEnemy;
    public GameObject prefabBomb;
    public float moveSpeed = 5f;
    public float interval = 10f;

    public int health = 1;
    public int maxHealth = 20;
    public bool isUseGun = true;
    public bool canShoot = false;

    public bool debugMode = false;

    private Vector2 move;
    private Vector2 lastMoveDir = Vector2.down;

    private Destroy destroyScript;
    private float timer;
    public bool isGoal = false;
    public int score = 10;
    private GameManager gameManager;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        destroyScript = GetComponent<Destroy>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        timer = interval;
        health += gameManager.enemyHpBonus;
    }

    void Update()
    {
        switch (enemyType)
        {
            case EnemyType.Dash:
                Dash();
                break;
            case EnemyType.Gun:
                Gun();
                break;
            case EnemyType.Bomb:
                Bomb();
                break;
        }
        // if (isUseGun && canShoot && !isGoal)
        // {
        //     if (timer <= 0f)
        //     {
        //         Vector2 bulletPos = transform.position;
        //         bulletPos.x -= 1f;
        //         Instantiate(prefabBulletEnemy, bulletPos, Quaternion.identity);
        //         timer = interval;
        //     }
        //     else
        //     {
        //         timer -= Time.deltaTime;
        //     }
        // }
    }

    void Dash()
    {
        if (player == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        lastMoveDir = direction;
        Vector2 animDir = lastMoveDir;
        animator.SetBool("IsMoving", true);
        animator.SetFloat("MoveX", animDir.x);
        animator.SetFloat("MoveY", animDir.y);

        spriteRenderer.flipX = animDir.x > 0;
    }

    void Gun()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Vector2 pos = transform.position;
            Instantiate(prefabBulletEnemy, pos, Quaternion.identity);
            timer = interval;
        }
    }

    void Bomb()
    {
        int moveDir = -1;
        float leftLimit = -20f;
        float rightLimit = 20f;

        Vector2 moveVector = moveDir == -1 ? Vector2.left : Vector2.right;
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);

        if (transform.position.x <= leftLimit)
        {
            moveDir = 1;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (transform.position.x >= rightLimit)
        {
            moveDir = -1;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Vector2 pos = transform.position;
            Instantiate(prefabBomb, pos, Quaternion.identity);
            timer = interval;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("bullet") && !isGoal)
        {
            Debug.Log("Hit");
            health--;
            if (health <= 0)
            {
                gameManager.score += score;
                destroyScript.Destroying();
            }
        }
    }
}