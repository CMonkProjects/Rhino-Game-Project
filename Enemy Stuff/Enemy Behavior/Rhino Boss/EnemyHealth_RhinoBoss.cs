using UnityEngine;

public class EnemyHealth_RhinoBoss : EnemyHealth
{
    [SerializeField] bool canTakeDamage = false;

    bool canEnterFinalAttackMode = true;
    [SerializeField] float lowOnHealthThreshold = 0.5f;

    [SerializeField] float maxDamagePerHit;
    [SerializeField] float armorDamageReduction;

    internal delegate void LowOnHealth();
    internal LowOnHealth lowOnHealth;

    public override void TakeDamage(float _damage)
    {
        if (!canTakeDamage)
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

        //Apply damage to both defense layers
        damageToShields = Mathf.Min(shields, shieldDamage);
        damageToHealth = healthDamage - Mathf.Max(0, shields);

        //Apply damage to shields
        if (damageToShields > 0)
        {
            shields -= damageToShields;

            //Call material shield flash method here
            DamageFlash(matBlue);
        }

        //Apply damage to Health
        if (damageToHealth > 0)
        {
            damageToHealth = ArmorDamageReduction(damageToHealth);      //Reduce damage dealt to boss

            Debug.LogError("DAMAGE TO BOSS IS - " + damageToHealth);

            health -= damageToHealth;

            //Call material damage flash method here
            DamageFlash(matYellow);
        }

        //Enter final attack mode if low on health
        if (canEnterFinalAttackMode)
        {
            if (health <= healthMax * lowOnHealthThreshold)
            {
                if (lowOnHealth != null)
                {
                    lowOnHealth();
                }

                canEnterFinalAttackMode = false;
                return;
            }
        }

        if (health <= healthMax * 0.6)
        {
            if (!heavilyDamaged)
            {
                StartCoroutine(HeavilyDamaged());
            }
        }

        if (health <= 0)
        {
            if (hasDyingDeathEffect)
            {
                StartCoroutine(Dying());
            }
            else
            {
                StartCoroutine(Die());
            }
            return;
        }

        if (shields < 0)
            shields = 0;

        SetHealthBar(true);
        Invoke("ResetMaterial", 0.1f);

        onTakenDamage.Invoke();     //OnTakenDamage event
    }

    //Reduce damage dealt to boss
    float ArmorDamageReduction(float _damage)
    {
        _damage -= armorDamageReduction;

        //if (_damage <= 0)
        //    _damage = 0;

        if (_damage > maxDamagePerHit)
            _damage = maxDamagePerHit;

        return _damage;
    }

    internal void CanTakeDamage(bool _bool)
    {
        canTakeDamage = _bool;
    }
}
