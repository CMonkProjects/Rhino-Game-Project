using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consequence_LRMLauncher : Consequence_Basic
{
    //Projectile
    [SerializeField] GameObject lrmProjectile;
    [SerializeField] float lrmProjectileForce;
    [SerializeField] Transform projectileLaunchPoint;

    [SerializeField] string objectiveTargetID;  //just input this manually

    //Particle effects
    [SerializeField] ParticleSystem[] particleEffect = new ParticleSystem[0];

    //launch projectile when countdown expired
    protected override void NegativeEvent()
    {
        base.NegativeEvent();

        GameObject projectile = Instantiate(lrmProjectile,
                projectileLaunchPoint.position,
                projectileLaunchPoint.rotation);

        Consequence_LRM _consequenceLRM = projectile.GetComponent<Consequence_LRM>();

        if (_consequenceLRM)
            _consequenceLRM.SetTargetID(objectiveTargetID);

        //Play the particle effect if we have one
        if (particleEffect!= null)
        {
            particleEffect[0].Play();
        }

        //Game Event LRM Launched
        GameEvents.current.LRMLaunched();

        GameEvents.current.NewMessage("Enemy LRM Launched");
    }
}
