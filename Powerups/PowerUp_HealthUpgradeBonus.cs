using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PowerUp_HealthUpgradeBonus : PowerUp
{
    protected override void CheckConditions()
    {
        base.CheckConditions();

        if (playerStats.healthMax >= playerStats.bonusHealthMax)
        {
            conditionResult = false;
            Debug.Log("Player maxed out on bonus health");
        }
        else
        {
            conditionResult = true;
        }
    }
    protected override void PowerupPayload()
    {
        base.PowerupPayload();

        if (playerStats)
        {
            playerStats.healthMax +=1;
            playerStats.PlayerHealed(playerStats.healthMax);
        }
    }
}
