using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This projectile inherits from Grenade, but the key difference is it doesn't make use of the proximity trigger
public class MegaCrusher : Grenade
{
    public GameObject megaCrushDamagePrefab;

    protected new void Start()
    {
        GameEvents.current.PlaySound(firingSound);

        var colliders = GetComponents<CircleCollider2D>();
        //collisionTrigger = colliders[0];
        proximityTrigger = colliders[0];

        currentTimer = basicTimer;
    }
    protected override void Update()
    {
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
        {
            Die();
        }

        if (currentTimer <= proximityTimer)
        {
            //Start Flashing
            if (!isFlashing)
            {
                StartCoroutine(StartFlashing());
            }
        }

        //Maintain the same speed even if our projectile bounces off a wall
        lastFrameVelocity = rb.velocity;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //The Grenade we inherit from would set off its proximity fuse, but instead we do nothing here
    }

    public override void Die()
    {
        if (aoeEffect != null)
        {
            Instantiate(aoeEffect, transform.position, Quaternion.identity);
        }

        //Here we instantiate the Damage Effect since it takes a short time to do its thing
        if (megaCrushDamagePrefab != null)
        {
            Instantiate(megaCrushDamagePrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
