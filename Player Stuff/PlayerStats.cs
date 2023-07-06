using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float health;
    public float healthMax = 5;
    public float bonusHealthMax;

    public GameObject deathEffect;
    SpriteRenderer spriteRenderer;
    public SpriteRenderer turretSpriteRenderer;
    public SpriteRenderer turretBlasterSpriteRenderer;

    public bool isInvulnerable = false; //prevents damage when hit and respawning

    void Start()
    {
        health = healthMax;
        GameEvents.current.PlayerHealthChanged();   //call event in game events
        GameEvents.current.PlayerSpawned();

        spriteRenderer = GetComponent<SpriteRenderer>();

        //Player blinks and is invulnerable for a few seconds
        StartCoroutine(TemporaryInvulnerability());
    }

    public void PlayerDamaged(float _damageAmount)
    {
        if (!isInvulnerable)
        {
            health -= _damageAmount;
            GameEvents.current.PlayerHealthChanged();   //call event in game events
            GameEvents.current.PlayerDamaged(_damageAmount);

            if (health <= 2 && health > 0)
            {
                GameEvents.current.PlayerLowHealth();
            }
            
            if (health <= 0)
            {
                Die();
                return;
            }

            //Player blinks and is invulnerable for a few seconds
            StartCoroutine(TemporaryInvulnerability());
        }
    }

    public void PlayerHealed(float _healthAmount)
    {
        health += _healthAmount;

        if (health > healthMax)
        {
            health = healthMax;
        }

        GameEvents.current.PlayerHealthChanged();
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        GameEvents.current.PlayerDied();

        Destroy(gameObject);
    }

    internal void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet" && !isInvulnerable)
        {
            Bullet bullet = collision.GetComponent<Bullet>();

            if (bullet != null)
            {
                PlayerDamaged(bullet.damage);
                bullet.Die();
            }
        }
    }

    //Called when player is damaged or when he spawns
    IEnumerator TemporaryInvulnerability()
    {
        isInvulnerable = true;

        for (int i = 0; i < 30; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            if (turretSpriteRenderer)
            {
                turretSpriteRenderer.enabled = !turretSpriteRenderer.enabled;
            }
            
            if (turretBlasterSpriteRenderer)
            {
                turretBlasterSpriteRenderer.enabled = !turretBlasterSpriteRenderer.enabled;
            }
            
            yield return new WaitForSeconds(0.1f);
        }

        isInvulnerable = false;
    }

    //Called by shield powerup
    public void PlayerShielded(bool _isShielded)
    {
        /*isShielded = _isShielded;

        if (playerShieldObject != null)
        {
            playerShieldObject.SetActive(isShielded);
        }*/
    }

    public void ShieldDestroyed()
    {
        //playerShieldEffect.ShieldDestroyed();
    }

    //Called by overcharge powerup
    public void PlayerOvercharge(bool _isOvercharged)
    {
        PlayerShootingRaycast playerShootingRaycast = GetComponentInChildren<PlayerShootingRaycast>();

        if (playerShootingRaycast != null)
        {
            playerShootingRaycast.Overcharge(_isOvercharged);
        }
    }
}
