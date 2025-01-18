using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Death : NetworkBehaviour
{
    public GameObject zoomCamera;
    public GameObject deathVFX;
    public float deathTime = 2f;
    public float zoomTime = 2f;
    
    void Start()
    {
        Health health = GetComponent<Health>();
        if (health == null) return;
        health.onPlayerDied += OnPlayerDied;
        if (zoomCamera == null) zoomCamera = GameObject.FindWithTag("MainCamera");
    }

    public void OnPlayerDied()
    {
        if (isClient) StartCoroutine(DeathAnimation());
    }

    public static float Ease(float t)
    {
        if(t < 0.5f) 
        return 8f * t * t * t * t;
        var x = -2 * t + 2;
        return 1 - x * x * x * x / 2;
    }

    IEnumerator DeathAnimation()
    {
        if (!isClient) yield break;

        var endTime = Time.time + zoomTime;
        Vector3 goal = transform.position;
        goal.z = zoomCamera.transform.position.z;
        var camera = zoomCamera.GetComponent<Camera>();
        var startSize = camera.orthographicSize;
        while (zoomCamera != null && Time.time < endTime)
        {
            zoomCamera.transform.position = Vector3.Lerp(zoomCamera.transform.position, goal, 0.005f);
            var zoomProgress = endTime == Time.time ? 1 : Mathf.Clamp01(1 - 1.3f * (endTime - Time.time) / zoomTime);
            camera.orthographicSize = Mathf.Lerp(startSize, 3, Ease(zoomProgress));
            yield return null;
        }

        if (deathVFX) Instantiate(deathVFX, transform.position, Quaternion.identity);
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;

        yield return new WaitForSeconds(deathTime);

        connectionToServer?.Disconnect(); // NetworkManagerのスクリプト確認。Disconnect以外でなにか、、
    }

    override public void OnStopClient()
    {
        SceneManager.LoadScene("Finish");
        var manager = NetworkManager.singleton;
        Destroy(manager.gameObject);
    }
}