using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp_ExtraLife : PowerUp
{
    public int extraLife = 1;

    protected override void PowerupPayload()
    {
        base.PowerupPayload();

        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        gameController.UpdateLives(extraLife);
    }
}
