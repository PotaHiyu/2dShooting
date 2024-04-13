using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownBomb : MonoBehaviour
{
    private float interval = 2f;
    private float timer = 0.0f;
    public GameObject bombPrefab;
    private Vector2 pos;

    void Update()
    {
        if (timer <= 0.0f)
        {
            pos = gameObject.transform.position;
            Instantiate(bombPrefab, pos, Quaternion.identity);
            timer = interval;
        }

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }
}
