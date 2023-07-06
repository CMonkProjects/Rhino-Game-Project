using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consequence_CommSignal : Consequence_Basic
{
    [SerializeField] string objectiveTargetID;  //just input this manually

    protected override void NegativeEvent()
    {
        base.NegativeEvent();

        GameEvents.current.EnemyCommSignal();

        GameEvents.current.TargetNotDestroyed(objectiveTargetID);

        GameEvents.current.NewMessage("Enemy Comm Signal Sent");

        //Debug Stuff
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = Color.red;
    }
}
