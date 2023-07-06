using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plasma : Bullet
{
    AOEDamage aoeDamage;

    protected override void Start()
    {
        GameEvents.current.PlaySound(firingSound);

        aoeDamage = GetComponent<AOEDamage>();
    }

    public override void Die()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        if (particleEmitter != null)
        {
            particleEmitter.transform.parent = null;
            particleEmitter.GetComponent<ParticleSystem>().Stop();
        }

        if (aoeDamage != null)
        {
            aoeDamage.Explode();
        }

        Destroy(gameObject);

    }
}
