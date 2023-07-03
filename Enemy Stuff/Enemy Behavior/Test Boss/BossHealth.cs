using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    [SerializeField] bool canBeDamaged;
    [SerializeField] float maxDamageLimiter;

    public void BossCanBeDamaged(bool _bool)
    {
        canBeDamaged = _bool;
    }

    public override void TakeDamage(float _damage)
    {
        if (!canBeDamaged)
        {
            return;
        }

        if (isDying)
        {
            return;
        }

        float damageToShields = 0;
        float damageToHealth = 0;

        var shieldDamage = _damage;
        var healthDamage = _damage;

        //Calculate damage to all 3 defense layers
        damageToShields = Mathf.Min(shields, shieldDamage);
        damageToHealth = healthDamage - Mathf.Max(0, shields);

        //Apply damage to shields
        if (damageToShields > 0)
        {
            if (damageToShields > maxDamageLimiter)
            {
                damageToShields = maxDamageLimiter;
            }

            shields -= damageToShields;

            //When we have a shield sprite object, tell the shield to flash colors
            DamageFlash(matBlue);
        }

        //Health
        if (damageToHealth > 0)
        {
            if (damageToHealth > maxDamageLimiter)
            {
                damageToHealth = maxDamageLimiter;
            }

            health -= damageToHealth;

            //Call material damage flash method here
            DamageFlash(matYellow);
        }

        if (health <= 0)
        {
            if (particleEmitterDying != null)
            {
                StartCoroutine(Dying());
            }
            else
            {
                StartCoroutine(Die());
            }
            return;
        }

        if (health <= healthMax / 3 && !heavilyDamaged)
        {
            StartCoroutine(HeavilyDamaged());
        }

        if (shields < 0)
            shields = 0;

        SetHealthBar(true);
        Invoke("ResetMaterial", 0.1f);

        onTakenDamage.Invoke();     //OnTakenDamage event
    }

    protected override IEnumerator Die()
    {
        transform.parent.GetComponent<BossBehavior_TestBoss>().BossDefeated();
        
        return base.Die();
    }
}
