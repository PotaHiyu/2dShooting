using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public enum AreaMode
    {
        DeathSpawnArea,
        ShootingRange
    }

    public AreaMode areaMode;

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (areaMode)
        {
            case AreaMode.DeathSpawnArea:
                Destroy(other.gameObject);
                break;
            case AreaMode.ShootingRange:
                EnemyManager enemyManager = other.gameObject.GetComponent<EnemyManager>();
                if (enemyManager != null)
                {
                    enemyManager.canShoot = true;
                }
                break;
        }
    }
}
