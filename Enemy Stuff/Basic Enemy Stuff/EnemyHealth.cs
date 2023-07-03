using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    protected float health;
    public float healthMax;

    protected float shields;
    public float shieldMax;

    bool damageOverTime = false;  //Just for taking continous damage from things like lasers
    protected bool heavilyDamaged = false;
    
    public GameObject deathEffect, particleEmitterHeavyDamage, particleEmitterWreckage;

    [Header("For larger enemies")]
    public bool hasDyingDeathEffect;
    public GameObject particleEmitterDying;
    public float dyingTime = 1f;
    protected bool isDying = false;

    [SerializeField] EnemyTurret enemyTurret;
    //Materials
    protected Material matYellow;
    protected Material matBlue;
    protected Material matDefault;
    public SpriteRenderer spriteRenderer;

    public UnityEvent onTakenDamage;
    public GameObject healthBarPrefab;
    protected GameObject healthBar;
    protected EnemyHealthbarBehavior healthbarScript;

    //Delegates
    public delegate void OnDestroyed();
    public OnDestroyed onDestroyed;
    public delegate void OnDying();
    public OnDying onDying;
    public delegate void OnDamageFlash(Material material);
    public OnDamageFlash onDamageFlash;
    public delegate void OnResetMaterial();
    public OnResetMaterial onResetMaterial;
    public delegate void IsDead();
    public IsDead onDead;

    [SerializeField] protected GameObject parentObject;

    protected virtual void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            matYellow = Resources.Load("YellowFlash", typeof(Material)) as Material;
            matBlue = Resources.Load("BlueFlash", typeof(Material)) as Material;
            matDefault = spriteRenderer.material;
        }
    }

    protected virtual void Start()
    {
        health = healthMax;
        shields = shieldMax;

        if (healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
            healthbarScript = healthBar.GetComponent<EnemyHealthbarBehavior>();
            healthbarScript.FollowEnemy(transform);
            SetHealthBar(false);
        }
    }
    public virtual void TakeDamage(float _damage)
    {
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

        //Health
        if (damageToHealth > 0)
        {
            health -= damageToHealth;

            //Call material damage flash method here
            DamageFlash(matYellow);
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

        if (health <= healthMax / 3 && !heavilyDamaged)
        {
            StartCoroutine(HeavilyDamaged());
        }

        if (shields < 0)
            shields = 0;

        SetHealthBar(true); //update the healthbar and make it visible
        Invoke("ResetMaterial", 0.1f);

        onTakenDamage.Invoke();     //OnTakenDamage event
    }

    //This is called by the player's laser weapon
    public virtual void TakeDamageOverTime(float _damage)
    {
        if (damageOverTime == true)
            return;

        damageOverTime = true;

        StartCoroutine(DamageOverTimeCoroutine(_damage));
    }

    protected virtual IEnumerator DamageOverTimeCoroutine(float _damage)
    {
        yield return new WaitForSeconds(0.15f);

        TakeDamage(_damage);
        damageOverTime = false;
    }

    //Set materials to yellow
    protected virtual void DamageFlash(Material _material)
    {
        spriteRenderer.material = _material;

        if (onDamageFlash != null)
        {
            onDamageFlash(_material);
        }
    }

    protected virtual void ResetMaterial()
    {
        spriteRenderer.material = matDefault;

        //delegate
        if (onResetMaterial != null)
        {
            onResetMaterial();
        }
    }

    protected virtual IEnumerator HeavilyDamaged()
    {
        heavilyDamaged = true;
        if (particleEmitterHeavyDamage != null)
        {
            particleEmitterHeavyDamage.SetActive(true);
            particleEmitterHeavyDamage.GetComponent<ParticleSystem>().Play();
        }

        while (true)
        {
            DamageFlash(matYellow);

            Invoke("ResetMaterial", 0.1f);

            yield return new WaitForSeconds(1f);
        }
    }

    protected virtual IEnumerator Die()
    {
        isDying = true;
        DamageFlash(matYellow);

        if (onDead != null)
        {
            onDead();   //call the delegate
        }

        yield return new WaitForSeconds(0.1f);

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        if (particleEmitterWreckage != null)
        {
            Instantiate(particleEmitterWreckage, transform.position, transform.rotation);
        }

        GameEvents.current.EnemyDestroyed();

        //call delegate
        if (onDestroyed != null)
        {
            onDestroyed();
        }

        if (parentObject != null)
        {
            Destroy(parentObject);
        }

        Destroy(gameObject);
    }

    //Similar to Die, but lasts a little longer and has an exploding particle effect
    protected virtual IEnumerator Dying()
    {
        isDying = true;
        DamageFlash(matYellow);

        //call delegate
        if (onDying != null)
        {
            onDying();
        }

        if (healthBar)
        {
            healthBar.SetActive(false);
        }

        if (particleEmitterDying != null)
        {
            GameObject dyingEffect = Instantiate(particleEmitterDying, transform.position, Quaternion.identity);
            
            if (dyingEffect)
            {
                dyingEffect.GetComponent<ParticleEffectAttacher>().AttachToObject(gameObject.transform);
                dyingEffect.GetComponent<ParticleEffectDying>().EmitParticles(dyingTime);
            }
        }

        yield return new WaitForSeconds(dyingTime);

        StartCoroutine(Die());
    }
    
    protected void SetHealthBar(bool _resetTimer)
    {
        if (healthBar != null)
        {
            healthbarScript.SetHealth(health, shields, _resetTimer);
        }
    }
}
