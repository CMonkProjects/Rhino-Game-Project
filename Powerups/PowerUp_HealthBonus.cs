using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp_HealthBonus : PowerUp
{
    public int healthBonus = 2;

    protected override void CheckConditions()
    {
        if (playerStats.health >= playerStats.healthMax)
        {
            conditionResult = false;
            Debug.Log("Player already at full health");
        }
        else
        {
            conditionResult = true;
        }
    }

    protected override void PowerupPayload()
    {
        base.PowerupPayload();

        if (playerStats != null)
        {
            playerStats.PlayerHealed(healthBonus);
        }
    }
}
