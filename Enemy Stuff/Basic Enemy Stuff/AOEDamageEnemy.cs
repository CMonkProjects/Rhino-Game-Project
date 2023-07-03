using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamageEnemy : AOEDamage
{
    [SerializeField] string hitTagEnemy = "Enemy";
    [SerializeField] string hitTagPlayer = "Player";
    [SerializeField] string hitTagDestructable = "Destructable";

    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask playerMask;

    bool damagePlayerOnce = false;

    void Start()
    {
        gameObject.transform.parent = null;
        Explode();
        Destroy(gameObject);
    }

    internal override void Explode()
    {
        //Check all enemies within the overlap circle and apply damage
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius, enemyMask);
        Collider2D[] colliderPlayer = Physics2D.OverlapCircleAll(transform.position, damageRadius, playerMask);

        foreach (Collider2D hit in colliders)
        {
            if (hit.tag == hitTagEnemy)
            {
                if (hit != null)
                {
                    hit.GetComponent<EnemyHealth>().TakeDamage(damage);
                }
            }

            if (hit.tag == hitTagDestructable)
            {
                if (hit != null)
                {
                    hit.GetComponent<DestructableHealth>().TakeDamage(damage);
                }
            }
        }

        foreach (Collider2D hit in colliderPlayer)
        {
            if (hit.tag == hitTagPlayer)
            {
                if (hit != null)
                {
                    hit.GetComponent<PlayerStats>().PlayerDamaged(damage);
                    damagePlayerOnce = true;
                }
            }
        }
    }
}
