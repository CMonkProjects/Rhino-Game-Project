using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    [Header("Particle Emitter is for things like missile smoke trails")]
    public GameObject particleEmitter;

    public float damage = 1;

    public string firingSound;

    public string enemyTag;

    protected virtual void Start()
    {
        GameEvents.current.PlaySound(firingSound);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == enemyTag)
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            Die();
        }
    }

    public virtual void Die()
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

        if (transform.parent != null)
        {
            Destroy(transform.parent);
        }

        Destroy(gameObject);
    }
}