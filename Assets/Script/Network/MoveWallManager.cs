using UnityEngine;
using Mirror;

public class MoveWallManager : NetworkBehaviour
{
    private float minY = -6.5f;
    private float maxY = 6.5f;
    [SerializeField] private float speed = 2f;
    [SyncVar] private float direction;
    void Start()
    {
        if (isServer)
        {
            direction = Random.value < 0.5f ? 1f : -1f;
            Vector3 pos = transform.position;
            pos.y = direction > 0 ? minY : maxY;
            transform.position = pos;
        }
    }

    void Update()
    {
        if (!isServer) return;
        Vector3 pos = transform.position;
        pos.y += direction * speed * Time.deltaTime;
        transform.position = pos;

        if (pos.y >= maxY)
        {
            pos.y = maxY;
            direction = -1;
        }
        else if (pos.y <= minY)
        {
            pos.y = minY;
            direction = 1;
        }

        transform.position = pos;
    }
}
