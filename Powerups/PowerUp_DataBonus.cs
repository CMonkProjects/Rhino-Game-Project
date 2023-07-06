using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp_DataBonus : PowerUp
{
    protected override void CheckConditions()
    {
        base.CheckConditions();
    }

    protected override void PowerupPayload()
    {
        base.PowerupPayload();

        GameEvents.current.HiddenBonus();
    }
}
