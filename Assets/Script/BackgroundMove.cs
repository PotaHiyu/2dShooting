using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    float start = 5.5f;
    float end = -5.5f;
    public float moveTime = 180f;
    float currentTime = 0f;
    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {
        startPos = new Vector3(start, 0, 0);
        endPos = new Vector3(end, 0, 0);
        transform.position = startPos;
    }

    void Update()
    {
        if (transform.position.x >= end)
        {
            currentTime += Time.deltaTime;
            float normalizedTime = currentTime / moveTime;
            transform.position = Vector3.Lerp(startPos, endPos, normalizedTime);
        }
    }
}
