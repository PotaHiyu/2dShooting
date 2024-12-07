using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class Death : NetworkBehaviour
{
    public GameObject zoomCamera;
    public float zoomTime = 2f;
    public GameObject deathVFX;
    public float deathTime = 2f;
    float deathTimeElapsed = 0f;

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
        if (t < 0.5f) return 8f * t * t * t * t;
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
            // Change the number to change the movement speed.
            zoomCamera.transform.position = Vector3.Lerp(zoomCamera.transform.position, goal, 0.005f);
            // Change the number to change the zoom start. 1 starts immediately; larger numbers delay the start.
            var zoomProgress = endTime == Time.time ? 1 : Mathf.Clamp01(1 - 1.3f * (endTime - Time.time) / zoomTime);
            // Change the number to change the zoom amount. Smaller numbers zoom in more.
            camera.orthographicSize = Mathf.Lerp(startSize, 3, Ease(zoomProgress));
            yield return null;
        }

        if (deathVFX) Instantiate(deathVFX, transform.position, Quaternion.identity);
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;

        yield return new WaitForSeconds(deathTime);

        Debug.Log($"Disconnecting! {isClient} {isServer} {isLocalPlayer} {connectionToServer}");
        connectionToServer?.Disconnect();
        // SceneManager.LoadScene("Title", LoadSceneMode.Additive);
    }

    override public void OnStopClient()
    {
        Debug.Log("got stop message");
        SceneManager.LoadScene("Title", LoadSceneMode.Additive);
    }
}
