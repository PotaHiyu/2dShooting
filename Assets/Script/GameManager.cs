using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using TMPro;

[System.Serializable]
public class SpawnInfo
{
    public GameObject objectToSpawn;
    public float xPosition;
    public float yPosition;
    public float spawnDelay;
}

public class GameManager : MonoBehaviour
{
    public bool isWin = false;
    public bool isLose = false;
    public float score = 0f;
    public GameObject clearText;
    public GameObject bulletIcon;
    public List<SpawnInfo> spawnInfos;
    private bool gameMode = ChooseMode.mode;

    void Start()
    {
        score = 0f;
        foreach (var info in spawnInfos)
        {
            StartCoroutine(SpawnAfterDelay(info));
        }
        if (gameMode == true)
        {
            bulletIcon.SetActive(true);
        }
    }

    void Update()
    {
        if (isWin)
        {
            clearText.SetActive(true);
            StartCoroutine(GameClear());
        }

        if (isLose)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator SpawnAfterDelay(SpawnInfo info)
    {
        yield return new WaitForSeconds(info.spawnDelay);
        Instantiate(info.objectToSpawn, new Vector2(info.xPosition, info.yPosition), info.objectToSpawn.transform.rotation);
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Title");
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Title");
    }
}
