using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableHealth : MonoBehaviour
{
    [SerializeField] protected float health;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Material material;
    protected Material matDefault;

    [SerializeField] protected GameObject particleEmitterExplode, particleEmitterRanOver, particleEmitterWreckage, wreckageRubble;

    [Header("Dying Effect")]
    [SerializeField] bool hasDyingDeathEffect;
    [SerializeField] GameObject particleEmitterDying;
    [SerializeField] float dyingEffectTime = 1f;

    [SerializeField] protected string playerBulletTag, enemyBulletTag, playerTag, enemyTag;

    [SerializeField] protected bool isBuilding, canBeRunOver, isTree;
    protected bool isExploding = false;

    [SerializeField] protected GameObject parentObject;

    public delegate void OnDestroyed();
    public OnDestroyed onDestroyed;

    [SerializeField] protected float damageFlashTime = 0.1f;
    bool damageOverTime = false;

    protected virtual void Awake()
    {
        matDefault = spriteRenderer.material;
    }

    //On trigger enter
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //if player bullet / enemy bullet
        if (collision.tag == playerBulletTag || collision.tag == enemyBulletTag)
        {
            float damageTaken = collision.GetComponent<Bullet>().damage;
            TakeDamage(damageTaken);
        }

        if (canBeRunOver && (collision.tag == playerTag || collision.tag == enemyTag))
        {
            RunOver();
        }
    }

    //Damage Flash
    protected virtual void DamageFlash()
    {
        spriteRenderer.material = material;
    }

    protected virtual void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    public virtual void TakeDamage(float _damage)
    {
        if (isExploding)
            return;

        StartCoroutine(TakenDamage(_damage));
    }

    protected virtual IEnumerator TakenDamage(float _damage)
    {
        DamageFlash();

        health -= _damage;

        if (health <= 0)
        {
            if (hasDyingDeathEffect)
            {
                StartCoroutine(Dying());
            }
            else if (!isExploding)
            {
                StartCoroutine(Explode());
            }

            yield break;
        }

        yield return new WaitForSeconds(damageFlashTime);

        ResetMaterial();
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

    public virtual void RunOver()
    {
        StartCoroutine(RanOver());
    }

    //Destroyed by being shot
    protected virtual IEnumerator Explode()
    {
        isExploding = true;

        DamageFlash();

        yield return new WaitForSeconds(damageFlashTime);

        //instantiate explosion
        if (particleEmitterExplode != null)
        {
            Instantiate(particleEmitterExplode, transform.position, Quaternion.identity);
        }

        DestroyedGenericStuff();
    }

    //Dying effect, works like the enemyHealth one
    protected virtual IEnumerator Dying()
    {
        isExploding = true;
        DamageFlash();

        /*//call delegate
        if (onDying != null)
        {
            onDying();
        }*/

        if (particleEmitterDying != null)
        {
            GameObject dyingEffect = Instantiate(particleEmitterDying, transform.position, Quaternion.identity);

            if (dyingEffect)
            {
                dyingEffect.GetComponent<ParticleEffectAttacher>().AttachToObject(gameObject.transform);
                dyingEffect.GetComponent<ParticleEffectDying>().EmitParticles(dyingEffectTime);
            }
        }

        yield return new WaitForSeconds(dyingEffectTime);

        StartCoroutine(Explode());
    }

    //Destroyed by being run over
    protected virtual IEnumerator RanOver()
    {
        yield return new WaitForSeconds(0.1f);

        //Explosion when ran over
        if (particleEmitterRanOver != null)
        {
            Instantiate(particleEmitterRanOver, transform.position, Quaternion.identity);
        }

        DestroyedGenericStuff();
    }

    protected virtual void DestroyedGenericStuff()
    {
        if (isBuilding)
        {
            GameEvents.current.BuildingDestroyed(); //call event
        }

        if (isTree)
        {
            GameEvents.current.TreeTerminated();
        }

        if (onDestroyed != null)
        {
            onDestroyed();  //call delegate
        }

        if (particleEmitterWreckage != null)
        {
            Instantiate(particleEmitterWreckage, transform.position, Quaternion.identity);
        }

        if (wreckageRubble)
        {
            Instantiate(wreckageRubble, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
