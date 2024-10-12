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

    IEnumerator DeathAnimation()
    {
        if (!isClient) yield break;

        var endTime = Time.time + zoomTime;
        Vector3 goal = transform.position + new Vector3(0, 0, -5);
        while (zoomCamera != null && Time.time < endTime)
        {
            zoomCamera.transform.position = Vector3.Lerp(zoomCamera.transform.position, goal, 0.025f);
            yield return null;
        }

        if (deathVFX) Instantiate(deathVFX, transform.position, Quaternion.identity);
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;

        yield return new WaitForSeconds(deathTime);

        Debug.Log("Disconnecting!");
        // connectionToServer.Disconnect();
        SceneManager.LoadScene("Title");
    }
}
