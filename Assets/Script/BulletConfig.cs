using UnityEngine;
using System;

[System.Serializable]
public class BulletSettings
{
    public float speed = 10f;           // 弾速
    public float fireRate = 0.5f;       // 発射間隔（秒）
    public Vector3 scale = Vector3.one; // 弾の大きさ
    public int damage = 1;              // ダメージ
    public bool piercing = false;       // 貫通弾
    public int multiShot = 1;           // 多弾発射（同時に撃つ弾数）
    public float spreadAngle = 0f;      // 拡散角度
    
    public BulletSettings()
    {
        speed = 10f;
        fireRate = 0.5f;
        scale = Vector3.one;
        damage = 1;
        piercing = false;
        multiShot = 1;
        spreadAngle = 0f;
    }
    
    public BulletSettings(float speed, float fireRate, Vector3 scale)
    {
        this.speed = speed;
        this.fireRate = fireRate;
        this.scale = scale;
        this.damage = 1;
        this.piercing = false;
        this.multiShot = 1;
        this.spreadAngle = 0f;
    }
    
    public BulletSettings Clone()
    {
        return new BulletSettings
        {
            speed = this.speed,
            fireRate = this.fireRate,
            scale = this.scale,
            damage = this.damage,
            piercing = this.piercing,
            multiShot = this.multiShot,
            spreadAngle = this.spreadAngle
        };
    }
}

public enum ItemType
{
    SpeedUp,        // 弾速アップ
    RapidFire,      // 連射速度アップ
    BigBullet,      // 弾サイズアップ
    SmallBullet,    // 弾サイズダウン（高速化）
    MultiShot,      // 多弾発射
    PiercingShot,   // 貫通弾
    PowerUp         // 総合パワーアップ
}

[System.Serializable]
public class ItemEffect
{
    public ItemType itemType;
    public float duration = 10f;       // 効果時間（0で永続）
    public float speedMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public Vector3 scaleMultiplier = Vector3.one;
    public int damageBonus = 0;
    public bool enablePiercing = false;
    public int multiShotBonus = 0;
    public float spreadAngleBonus = 0f;
}

public class BulletConfig : MonoBehaviour
{
    public BulletSettings defaultSettings;
    public BulletSettings currentSettings;
    public System.Collections.Generic.List<ItemEffect> activeEffects = new System.Collections.Generic.List<ItemEffect>();
    
    private System.Collections.Generic.Dictionary<ItemType, float> effectTimers = new System.Collections.Generic.Dictionary<ItemType, float>();
    public System.Action<BulletSettings> OnBulletSettingsChanged;
    
    void Start()
    {
        if (defaultSettings == null)
        {
            defaultSettings = new BulletSettings();
        }
        
        currentSettings = defaultSettings.Clone();
    }
    
    void Update()
    {
        UpdateEffectTimers();
    }
    
    public void ApplyItemEffect(ItemEffect effect)
    {
        Debug.Log($"アイテム効果適用: {effect.itemType}");
        
        activeEffects.RemoveAll(e => e.itemType == effect.itemType);
        activeEffects.Add(effect);
        
        if (effect.duration > 0)
        {
            effectTimers[effect.itemType] = effect.duration;
        }
        
        UpdateCurrentSettings();
    }
    
    private void UpdateEffectTimers()
    {
        var expiredEffects = new System.Collections.Generic.List<ItemType>();
        var timerKeys = new System.Collections.Generic.List<ItemType>(effectTimers.Keys);
        
        foreach (var effectType in timerKeys)
        {
            effectTimers[effectType] -= Time.deltaTime;
            
            if (effectTimers[effectType] <= 0)
            {
                expiredEffects.Add(effectType);
            }
        }
        
        foreach (var expiredType in expiredEffects)
        {
            RemoveEffect(expiredType);
        }
    }
    
    public void RemoveEffect(ItemType effectType)
    {
        activeEffects.RemoveAll(e => e.itemType == effectType);
        effectTimers.Remove(effectType);
        UpdateCurrentSettings();
        
        Debug.Log($"アイテム効果終了: {effectType}");
    }
    
    private void UpdateCurrentSettings()
    {
        currentSettings = defaultSettings.Clone();
        
        foreach (var effect in activeEffects)
        {
            ApplyEffectToSettings(currentSettings, effect);
        }
        
        OnBulletSettingsChanged?.Invoke(currentSettings);
    }
    
    private void ApplyEffectToSettings(BulletSettings settings, ItemEffect effect)
    {
        settings.speed *= effect.speedMultiplier;
        settings.fireRate *= effect.fireRateMultiplier;
        settings.scale = Vector3.Scale(settings.scale, effect.scaleMultiplier);
        settings.damage += effect.damageBonus;
        
        if (effect.enablePiercing)
            settings.piercing = true;
        
        settings.multiShot += effect.multiShotBonus;
        settings.spreadAngle += effect.spreadAngleBonus;
        
        settings.speed = Mathf.Max(1f, settings.speed);
        settings.fireRate = Mathf.Max(0.1f, settings.fireRate);
        settings.scale = new Vector3(
            Mathf.Max(0.1f, settings.scale.x),
            Mathf.Max(0.1f, settings.scale.y),
            Mathf.Max(0.1f, settings.scale.z)
        );
        settings.damage = Mathf.Max(1, settings.damage);
        settings.multiShot = Mathf.Max(1, settings.multiShot);
    }
    
    public void ResetToDefault()
    {
        activeEffects.Clear();
        effectTimers.Clear();
        currentSettings = defaultSettings.Clone();
        OnBulletSettingsChanged?.Invoke(currentSettings);
        
        Debug.Log("弾設定をデフォルトにリセット");
    }
    
    public BulletSettings GetCurrentSettings()
    {
        return currentSettings;
    }
}