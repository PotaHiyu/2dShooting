using UnityEngine;
using Mirror;

public class MoveWallManager : NetworkBehaviour
{
    public OnlineGameManager ogm;
    private float minY = -6.5f;
    private float maxY = 6.5f;
    private bool isMoving = false;
    [SerializeField] private float speed = 2f;
    [SyncVar] private float direction;
    void Start()
    {
        if (ogm != null)
        {
            ogm.onGameStateChanged += OnGameStateChanged;
        }
        if (isServer)
        {
            direction = Random.value < 0.5f ? 1f : -1f;
            transform.position = Vector3.zero;
            // Vector3 pos = transform.position;
            // pos.y = direction > 0 ? minY : maxY;
            // transform.position = pos;
        }
    }

    void OnDestroy()
    {
        if (ogm != null)
        {
            ogm.onGameStateChanged -= OnGameStateChanged;
        }
    }

    private void OnGameStateChanged(GameState state)
    {
        isMoving = (state == GameState.Playing);
    }

    void Update()
    {
        if (isServer) {
            Vector3 pos = transform.position;
            pos.y += direction * speed * Time.deltaTime;

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
}
