using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OfflineItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public int itemsToSpawn = 5;
    public float spawnInterval = 15f;
    public float wallPadding = 1.5f;
    
    private Vector2 areaMin;
    private Vector2 areaMax;
    private List<GameObject> activeItems = new List<GameObject>();

    void Start()
    {
        areaMin = new Vector2(-10f + wallPadding, -5.5f + wallPadding);
        areaMax = new Vector2(10f - wallPadding, 5.5f - wallPadding);
        
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(5f);
        
        while (true)
        {
            SpawnItems();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnItems()
    {
        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < itemsToSpawn; i++)
        {
            Vector3 position = GetRandomPosition(usedPositions);
            SpawnItemAt(position);
            usedPositions.Add(position);
        }
    }

    Vector3 GetRandomPosition(List<Vector3> excludePositions)
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
            Debug.LogError("OfflineItemSpawner: itemPrefab is null!");
            return;
        }
        
        GameObject item = Instantiate(itemPrefab, position, Quaternion.identity);
        
        if (item != null)
        {
            activeItems.Add(item);
            StartCoroutine(AutoDestroyItem(item, 15f));
        }
    }

    IEnumerator AutoDestroyItem(GameObject item, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        if (item != null)
        {
            activeItems.Remove(item);
            Destroy(item);
        }
    }

    public void ClearAllItems()
    {
        foreach (var item in activeItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        activeItems.Clear();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Vector3 center = new Vector3((areaMin.x + areaMax.x) / 2, (areaMin.y + areaMax.y) / 2, 0);
        Vector3 size = new Vector3(areaMax.x - areaMin.x, areaMax.y - areaMin.y, 0.1f);
        Gizmos.DrawCube(center, size);
        Gizmos.DrawWireCube(center, size);
    }
}