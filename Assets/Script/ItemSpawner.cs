using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> itemPrefabs = new List<GameObject>();
    public float spawnInterval = 15f;
    public int maxItemsOnScreen = 3;
    
    // 1-1シーン用のスポーン範囲
    public Vector2 singlePlayerSpawnMin = new Vector2(-8f, -4f);
    public Vector2 singlePlayerSpawnMax = new Vector2(8f, 4f);
    
    // オンラインシーン用のプレイヤー陣地範囲
    public Vector2 player1TerritoryMin = new Vector2(-8f, -4f);
    public Vector2 player1TerritoryMax = new Vector2(-1f, 4f);
    public Vector2 player2TerritoryMin = new Vector2(1f, -4f);
    public Vector2 player2TerritoryMax = new Vector2(8f, 4f);
    
    private List<GameObject> spawnedItems = new List<GameObject>();
    private float spawnTimer = 0f;
    private bool isOnlineMode = false;
    
    private void Start()
    {
        // シーン名でオンラインモードを判定
        string sceneName = SceneManager.GetActiveScene().name;
        isOnlineMode = sceneName == "Online" || sceneName == "1vs1";
        
        spawnTimer = spawnInterval;
    }
    
    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        
        // 破壊されたアイテムをリストから削除
        spawnedItems.RemoveAll(item => item == null);
        
        // アイテムをスポーンする条件をチェック
        if (spawnTimer <= 0f && spawnedItems.Count < maxItemsOnScreen && itemPrefabs.Count > 0)
        {
            SpawnRandomItem();
            spawnTimer = spawnInterval;
        }
    }
    
    private void SpawnRandomItem()
    {
        GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
        Vector2 spawnPosition;
        
        if (isOnlineMode)
        {
            // オンラインモード: プレイヤーの陣地内にスポーン
            spawnPosition = GetOnlineSpawnPosition();
        }
        else
        {
            // シングルプレイモード: ステージ上にランダムスポーン
            spawnPosition = GetSinglePlayerSpawnPosition();
        }
        
        GameObject spawnedItem = Instantiate(randomItemPrefab, spawnPosition, Quaternion.identity);
        spawnedItems.Add(spawnedItem);
        
        Debug.Log($"アイテムスポーン: {randomItemPrefab.name} at {spawnPosition}");
    }
    
    private Vector2 GetSinglePlayerSpawnPosition()
    {
        float x = Random.Range(singlePlayerSpawnMin.x, singlePlayerSpawnMax.x);
        float y = Random.Range(singlePlayerSpawnMin.y, singlePlayerSpawnMax.y);
        return new Vector2(x, y);
    }
    
    private Vector2 GetOnlineSpawnPosition()
    {
        // オンラインモードでは各プレイヤーの陣地内にアイテムをスポーン
        // 現在は単純にランダムで選択、実際には各プレイヤー用に分けて管理する必要がある
        bool spawnForPlayer1 = Random.Range(0, 2) == 0;
        
        if (spawnForPlayer1)
        {
            float x = Random.Range(player1TerritoryMin.x, player1TerritoryMax.x);
            float y = Random.Range(player1TerritoryMin.y, player1TerritoryMax.y);
            return new Vector2(x, y);
        }
        else
        {
            float x = Random.Range(player2TerritoryMin.x, player2TerritoryMax.x);
            float y = Random.Range(player2TerritoryMin.y, player2TerritoryMax.y);
            return new Vector2(x, y);
        }
    }
    
    // 特定の位置にアイテムを手動スポーン
    public void SpawnItemAtPosition(ItemType itemType, Vector2 position)
    {
        GameObject itemPrefab = itemPrefabs.Find(prefab => 
        {
            ItemPickup pickup = prefab.GetComponent<ItemPickup>();
            return pickup != null && pickup.itemType == itemType;
        });
        
        if (itemPrefab != null)
        {
            GameObject spawnedItem = Instantiate(itemPrefab, position, Quaternion.identity);
            spawnedItems.Add(spawnedItem);
        }
    }
    
    // プレイヤー固有の陣地にアイテムをスポーン（オンライン用）
    public void SpawnItemForPlayer(ItemType itemType, int playerNumber)
    {
        if (!isOnlineMode) return;
        
        Vector2 spawnPosition;
        if (playerNumber == 1)
        {
            float x = Random.Range(player1TerritoryMin.x, player1TerritoryMax.x);
            float y = Random.Range(player1TerritoryMin.y, player1TerritoryMax.y);
            spawnPosition = new Vector2(x, y);
        }
        else
        {
            float x = Random.Range(player2TerritoryMin.x, player2TerritoryMax.x);
            float y = Random.Range(player2TerritoryMin.y, player2TerritoryMax.y);
            spawnPosition = new Vector2(x, y);
        }
        
        SpawnItemAtPosition(itemType, spawnPosition);
    }
}