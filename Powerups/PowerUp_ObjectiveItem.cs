using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp_ObjectiveItem : PowerUp
{
    public string objectiveItemType;

    protected override void PowerupPayload()
    {
        base.PowerupPayload();

        GameEvents.current.CollectedObjectiveItem(objectiveItemType);
    }
}
