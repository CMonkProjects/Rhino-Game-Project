using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp_Item : PowerUp
{
    protected override void PowerupPayload()
    {
        base.PowerupPayload();

        GameEvents.current.CollectedItem();
    }
}
