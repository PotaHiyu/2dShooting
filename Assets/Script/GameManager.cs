using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class SpawnInfo
{
    public GameObject objectToSpawn;
    public Vector2 position;
    public float spawnDelay;
}

[System.Serializable]
public class SpawnPattern
{
    public List<SpawnInfo> spawns = new List<SpawnInfo>();
}

public class GameManager : MonoBehaviour
{
    public bool isWin = false;
    public bool isLose = false;
    public float score = 0f;
    public GameObject clearText;
    public GameObject bulletIcon;
    public List<SpawnPattern> patterns = new List<SpawnPattern>();
    public float patternInterval = 3f;
    public float speedMultiplier = 1f;
    public float intervalMultiplier = 1f;
    public float speedIncreasePerScore = 0.01f;
    public float intervalDecreasePerScore = 0.01f;
    public int enemyHpBonus = 0;
    public string gameOverSceneName = "Finish";
    private bool gameMode = ChooseMode.mode;
    public TextMeshProUGUI scoreText;
    private TextMeshProUGUI scoreTextCom;
    private DontDestroyOnLoad dontDestroyObj;
    private bool hasGameOverStarted = false;

    void Start()
    {
        score = 0f;
        if (gameMode)
            bulletIcon.SetActive(true);

        StartCoroutine(SpawnLoop());
        isLose = false;
        isWin = false;
        scoreTextCom = clearText.GetComponent<TextMeshProUGUI>();
        dontDestroyObj = GameObject.Find("DontDestroyOnLoad").GetComponent<DontDestroyOnLoad>();
    }

    void Update()
    {
        if (!isWin && !isLose)
        {
            score += Time.deltaTime;
        }
        speedMultiplier += speedIncreasePerScore * Time.deltaTime;
        intervalMultiplier += intervalDecreasePerScore * Time.deltaTime;

        enemyHpBonus = Mathf.FloorToInt(score / 200f);

        if (isWin)
        {
            clearText.SetActive(true);
            StartCoroutine(GameClear());
        }
        if (isLose)
        {
            StartCoroutine(GameOver());
        }
        
        if (!hasGameOverStarted)
        {
            scoreText.text = Mathf.FloorToInt(score).ToString();
        }
    }

    IEnumerator SpawnLoop()
    {
        while(!isLose && !isWin)
        {
            if (patterns.Count == 0)
                yield break;

            int index = Random.Range(0, patterns.Count);
            SpawnPattern pattern = patterns[index];

            foreach (var spawnInfo in pattern.spawns)
            {
                yield return new WaitForSeconds(spawnInfo.spawnDelay);

                GameObject obj = Instantiate(
                    spawnInfo.objectToSpawn,
                    spawnInfo.position,
                    spawnInfo.objectToSpawn.transform.rotation
                );

                EnemyMove enemyMove = obj.GetComponent<EnemyMove>();
                if (enemyMove != null)
                {
                    enemyMove.moveSpeed *= speedMultiplier;
                }
            }

            float adjustedInterval = patternInterval / intervalMultiplier;
            yield return new WaitForSeconds(adjustedInterval);

            // speedMultiplier += speedIncreasePerScore * score;
            // intervalMultiplier += intervalDecreasePerScore * score;
        }
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Title");
    }

    IEnumerator GameOver()
    {
        if (hasGameOverStarted)
        yield break;
    
        hasGameOverStarted = true;
        Debug.Log("GAME OVER!!!");
        dontDestroyObj.score = score;
        clearText.SetActive(true);
        scoreTextCom.text = "GAME OVER?\n\nYour score\n" + Mathf.FloorToInt(score).ToString();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(gameOverSceneName);
    }
}