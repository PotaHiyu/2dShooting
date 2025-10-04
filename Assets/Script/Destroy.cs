using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public bool isObject = false;
    const float DESTROY_TIME = 0.5f;
    public bool isAnimationFinished = false;
    private GameManager gameManager;
    private GameObject player;
    public bool isPlayer = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (!isObject)
        {
            StartCoroutine(DestroyTime());
        }
        GameObject gameManagerObj = GameObject.FindWithTag("GameController");
        gameManager = gameManagerObj.GetComponent<GameManager>();
        Debug.Log(gameManager.isLose);
    }

    private void Update()
    {
        if (isAnimationFinished)
        {
            Destroying();
        }
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(DESTROY_TIME);
        Destroying();
    }

    public void Destroying()
    {
        if (isPlayer)
        {
            gameManager.isLose = true;
            player.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
