using UnityEngine;
using Mirror;

public class CollectableItem : NetworkBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            CollectItem(other.gameObject);
        }
    }

    void CollectItem(GameObject player)
    {
        // RpcPlayCollectEffect();
        NetworkServer.Destroy(gameObject);
    }

    // [ClientRpc]
    // void RpcPlayCollectEffect()
    // {
    //     if (isServer) return;
    //     GetComponent<AudioSource>().Play();
    // }
}
