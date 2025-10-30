using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemType itemType = ItemType.SpeedUp;
    public float duration = 10f;
    
    public float speedMultiplier = 1.5f;
    public float fireRateMultiplier = 1f;
    public Vector3 scaleMultiplier = Vector3.one;
    public int damageBonus = 0;
    public bool enablePiercing = false;
    public int multiShotBonus = 0;
    public float spreadAngleBonus = 0f;
    
    public AudioClip pickupSound;
    
    private void Start()
    {
        SetDefaultEffectsByType();
    }
    
    private void SetDefaultEffectsByType()
    {
        switch (itemType)
        {
            case ItemType.SpeedUp:
                speedMultiplier = 1.5f;
                break;
            case ItemType.RapidFire:
                fireRateMultiplier = 0.5f;
                break;
            case ItemType.BigBullet:
                scaleMultiplier = new Vector3(1.5f, 1.5f, 1.5f);
                damageBonus = 1;
                break;
            case ItemType.SmallBullet:
                scaleMultiplier = new Vector3(0.7f, 0.7f, 0.7f);
                speedMultiplier = 1.3f;
                break;
            case ItemType.MultiShot:
                multiShotBonus = 2;
                spreadAngleBonus = 15f;
                break;
            case ItemType.PiercingShot:
                enablePiercing = true;
                damageBonus = 1;
                break;
            case ItemType.PowerUp:
                speedMultiplier = 1.3f;
                fireRateMultiplier = 0.7f;
                scaleMultiplier = new Vector3(1.2f, 1.2f, 1.2f);
                damageBonus = 1;
                break;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BulletConfig bulletConfig = other.GetComponent<BulletConfig>();
            if (bulletConfig == null)
            {
                Move playerMove = other.GetComponent<Move>();
                if (playerMove != null)
                {
                    bulletConfig = playerMove.GetComponent<BulletConfig>();
                }
            }
            
            if (bulletConfig != null)
            {
                ItemEffect effect = new ItemEffect
                {
                    itemType = this.itemType,
                    duration = this.duration,
                    speedMultiplier = this.speedMultiplier,
                    fireRateMultiplier = this.fireRateMultiplier,
                    scaleMultiplier = this.scaleMultiplier,
                    damageBonus = this.damageBonus,
                    enablePiercing = this.enablePiercing,
                    multiShotBonus = this.multiShotBonus,
                    spreadAngleBonus = this.spreadAngleBonus
                };
                
                bulletConfig.ApplyItemEffect(effect);
                
                if (pickupSound != null && AudioSource.FindObjectOfType<AudioSource>() != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }
                
                Debug.Log($"アイテム取得: {itemType}");
                
                Destroy(gameObject);
            }
        }
    }
}