using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int itemsPerSide = 5; // 各陣地n個ずつ
    [SerializeField] private float spawnInterval = 15f; // n秒ごとにスポーン

    [SerializeField] private float wallPadding = 1.5f;
    private Vector2 leftAreaMin;
    private Vector2 leftAreaMax;
    private Vector2 rightAreaMin;
    private Vector2 rightAreaMax;

    private List<GameObject> activeItems = new List<GameObject>();
    private OnlineGameManager ogm;

    void Awake()
    {
        leftAreaMin = new Vector2(-10f + wallPadding, -5.5f + wallPadding);
        leftAreaMax = new Vector2(0f - wallPadding / 2, 5.5f - wallPadding);

        rightAreaMin = new Vector2(0f + wallPadding / 2, -5.5f + wallPadding);
        rightAreaMax = new Vector2(10f - wallPadding, 5.5f - wallPadding);
    }

    void Start()
    {
        if (!isServer) return;
        ogm = FindAnyObjectByType<OnlineGameManager>();
        ogm.onGameStateChanged += OnGameStateChanged;
    }

    void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Playing)
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    void Update() {
        if (isServer && Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Manual spawn triggered");
            SpawnSymmetricItems();
        }
    }

    IEnumerator SpawnRoutine()
    {
        Debug.Log("ItemSpawner: Waiting 5 seconds before first spawn");
        yield return new WaitForSeconds(5f);
        Debug.Log("ItemSpawner: Starting spawn loop");
        while (true)
        {
            Debug.Log("ItemSpawner: Spawning symmetric items");
            SpawnSymmetricItems();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    [Server]
    void SpawnSymmetricItems()
    {
        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < itemsPerSide; i++)
        {
            Vector3 leftPos = GetRandomPosition(leftAreaMin, leftAreaMax, usedPositions);
            SpawnItemAt(leftPos);
            usedPositions.Add(leftPos);

            Vector3 rightPos = GetRandomPosition(rightAreaMin, rightAreaMax, usedPositions);
            SpawnItemAt(rightPos);
            usedPositions.Add(rightPos);
        }
    }

    Vector3 GetRandomPosition(Vector2 areaMin, Vector2 areaMax, List<Vector3> excludePositions)
    {
        Vector3 position;
        int attempts = 0;
        float minDistance = 1.5f;

        do
        {
            float x = Random.Range(areaMin.x, areaMax.x);
            float y = Random.Range(areaMin.y, areaMax.y);
            position = new Vector3(x, y, 0);

            attempts++;
            if (attempts > 50)
            {
                break;
            }
        }
        while (!IsPositionValid(position, excludePositions, minDistance));
        return position;
    }

    bool IsPositionValid(Vector3 position, List<Vector3> excludePositions, float minDistance)
    {
        foreach (var pos in excludePositions)
        {
            if (Vector3.Distance(position, pos) < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    void SpawnItemAt(Vector3 position)
    {
        if (itemPrefab == null)
        {
            Debug.LogError("ItemSpawner: itemPrefab is null!");
            return;
        }
        
        // NetworkIdentityチェック
        if (itemPrefab.GetComponent<NetworkIdentity>() == null)
        {
            Debug.LogError($"ItemSpawner: {itemPrefab.name} does not have NetworkIdentity component! Cannot spawn network object.");
            return;
        }
        
        Debug.Log($"ItemSpawner: Spawning item at position {position}");
        GameObject item = Instantiate(itemPrefab, position, Quaternion.identity);
        
        if (item != null)
        {
            Debug.Log($"ItemSpawner: Item instantiated successfully: {item.name}");
            try
            {
                NetworkServer.Spawn(item);
                activeItems.Add(item);
                StartCoroutine(AutoDestroyItem(item, 15f));
                Debug.Log($"ItemSpawner: Successfully spawned item. Active items count: {activeItems.Count}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"ItemSpawner: Failed to NetworkServer.Spawn: {e.Message}");
                Destroy(item);
            }
        }
        else
        {
            Debug.LogError("ItemSpawner: Failed to instantiate item!");
        }
    }

    IEnumerator AutoDestroyItem(GameObject item, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        if (item != null)
        {
            activeItems.Remove(item);
            NetworkServer.Destroy(item);
        }
    }

    public void ClearAllItems()
    {
        if (!isServer) return;
        foreach (var item in activeItems)
        {
            if (item != null)
            {
                NetworkServer.Destroy(item);
            }
        }
        activeItems.Clear();
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Awake();
        }
        Gizmos.color = new Color(0, 0, 1, 0.3f);
        DrawArea(leftAreaMin, leftAreaMax);

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        DrawArea(rightAreaMin, rightAreaMax);
    }

    void DrawArea(Vector2 min, Vector2 max)
    {
        Vector3 center = new Vector3((min.x + max.x) / 2, (min.y + max.y) / 2, 0);
        Vector3 size = new Vector3(max.x - min.x, max.y, 0.1f);
        Gizmos.DrawCube(center, size);
        Gizmos.DrawWireCube(center, size);
    }
}
