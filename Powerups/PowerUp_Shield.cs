using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp_Shield : PowerUp
{
    public float shieldHealth = 5;

    public float shieldTimer = 10f;

    //Shield image effect
    //public GameObject shieldSprite;

    //Shield UI Icon
    protected override void Start()
    {
        base.Start();

        GameEvents.current.onPlayerDamaged += ShieldDamaged;
    }

    private void OnDestroy()
    {
        GameEvents.current.onPlayerDamaged -= ShieldDamaged;
    }
    protected override void PowerupPayload()
    {
        base.PowerupPayload();

        if (playerStats != null)
        {
            playerStats.PlayerShielded(true);
        }

        /*if (shieldSprite != null)
        {
            shieldSprite.SetActive(true);
        }*/
    }

    protected override void PowerupHasExpired()
    {
        if (playerStats != null)
        {
            playerStats.PlayerShielded(false);
            playerStats.ShieldDestroyed();
        }

        base.PowerupHasExpired();
    }

    // Update is called once per frame
    void Update()
    {
        if (powerupState == PowerupState.Collected)
        {
            //shieldTimer -= Time.deltaTime;

            if (shieldHealth <= 0)
            {
                PowerupHasExpired();
                /*if (shieldSprite != null)
                {
                    shieldSprite.SetActive(false);
                }*/
            }
        }
    }

    void ShieldDamaged(float _damageAmount)
    {
        if (powerupState == PowerupState.Collected)
        {
            shieldHealth -= _damageAmount;
        }
    }
}
