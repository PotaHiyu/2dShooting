using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public bool isObject = false;
    const float DESTROY_TIME = 0.5f;
    public int tradeScore = 0;
    public bool isAnimationFinished = false;
    public GameManager gameManager;
    public GameObject player;

    void Start()
    {
        if (!isObject)
        {
            StartCoroutine(DestroyTime());
        }
    }

    private void Update()
    {
        if (isAnimationFinished)
        {
            TradeScore();
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
        if (gameObject == player)
        {
            gameManager.isLose = true;
        }
        Destroy(gameObject);
    }

    public void TradeScore()
    {

        tradeScore = 20;
    }
}
