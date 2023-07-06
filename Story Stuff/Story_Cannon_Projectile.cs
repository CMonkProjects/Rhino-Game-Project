using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_Cannon_Projectile : Consequence_LRM
{
    protected override void NegativeEvent()
    {
        firedNegativeEvent = true;

        GameEvents.current.NewMessage("Superweapon shell flying to target");

        UnparentParticleEmitter();

        Destroy(gameObject);
    }
}
