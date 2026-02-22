using UnityEngine;

public class OfflineCollectableItem : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectItem(other.gameObject);
        }
    }

    void CollectItem(GameObject player)
    {
        Move playerMove = player.GetComponent<Move>();
        if (playerMove != null)
        {
            playerMove.ApplyPowerUp();
        }
        
        Destroy(gameObject);
    }
}