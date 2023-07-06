using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp_Overcharge : PowerUp
{
    public float overChargeTimer = 10f;

    protected override void PowerupPayload()
    {
        base.PowerupPayload();

        if (playerStats != null)
        {
            playerStats.PlayerOvercharge(true);
        }
    }
    protected override void PowerupHasExpired()
    {
        if (playerStats != null)
        {
            playerStats.PlayerOvercharge(false);
        }

        base.PowerupHasExpired();
    }
    // Update is called once per frame
    void Update()
    {
        if (powerupState == PowerupState.Collected)
        {
            overChargeTimer -= Time.deltaTime;

            if (overChargeTimer <= 0f)
            {
                PowerupHasExpired();
            }
        }
    }
}
