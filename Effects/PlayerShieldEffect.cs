using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldEffect : MonoBehaviour
{
    Material matWhite;
    Material matDefault;
    SpriteRenderer spriteRenderer;
    PlayerStats playerStats;
    [SerializeField] GameObject shieldExplosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;
        playerStats = GetComponent<PlayerStats>();

        GameEvents.current.onPlayerDamaged += ShieldDamaged;
    }

    private void OnDestroy()
    {
        GameEvents.current.onPlayerDamaged -= ShieldDamaged;
    }

    //Called by player stats
    internal void StartShieldEffect()
    {
        //spriteRenderer.material = matWhite;
    }

    internal void ShieldDamaged(float _damage)
    {
        spriteRenderer.material = matWhite;

        Invoke("ResetMaterial", 0.1f);
    }

    internal void ShieldDestroyed()
    {
        if (shieldExplosionPrefab != null)
        {
            Instantiate(shieldExplosionPrefab, transform.position, Quaternion.identity);
        }
    }

    void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }
}
