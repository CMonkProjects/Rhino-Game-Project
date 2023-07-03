using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDropper : MonoBehaviour
{
    [SerializeField] GameObject powerup;

    [SerializeField] DestructableHealth destructableHealth;
    [SerializeField] EnemyHealth enemyHealth;

    void OnEnable()
    {
        if (destructableHealth == null)
        {
            destructableHealth = GetComponent<DestructableHealth>();
        }

        if (enemyHealth == null)
        {
            enemyHealth = GetComponent<EnemyHealth>();
        }

        if (destructableHealth)
        {
            destructableHealth.onDestroyed += Destroyed;
        }
            
        if (enemyHealth)
        {
            enemyHealth.onDestroyed += Destroyed;
        }
    }

    void OnDisable()
    {
        if (destructableHealth)
            destructableHealth.onDestroyed -= Destroyed;
        else if (enemyHealth)
        {
            enemyHealth.onDestroyed -= Destroyed;
        }
    }

    void Destroyed()
    {
        if (powerup != null)
        {
            Debug.LogError("DROPPING POWERUP " + powerup.name);
            Instantiate(powerup, transform.position, transform.rotation);
        }
    }

    /*void OnDestroy()
    {
        if (powerup && dropPowerup)
        {
            Instantiate(powerup, transform.position, transform.rotation);
        }
    }*/
}