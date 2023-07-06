using UnityEngine;
using System.Collections;

public class Grenade : Bullet
{
    protected Rigidbody2D rb;
    protected float minVelocity = 0.5f; //0.8f default
    protected Vector2 initialVelocity;
    protected Vector2 lastFrameVelocity;

    public GameObject aoeEffect;

    public string bouncingSound;

    [Tooltip("The basic countdown timer for the grenade")]
    public float basicTimer;
    [Tooltip("The short timer that goes off when grenade is near an enemy")]
    public float proximityTimer;
    protected float currentTimer;
    protected bool proximityFuseActive = false;
    protected bool isFlashing = false;

    AOEDamage aoeDamage;
    protected CircleCollider2D proximityTrigger;
    BoxCollider2D grenadeCollider;

    //Materials and animation
    protected Material matYellow;
    protected Material matDefault;
    protected SpriteRenderer spriteRenderer;
    Animator spriteAnimator;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        matYellow = Resources.Load("YellowFlash", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;
        spriteAnimator = GetComponent<Animator>();
    }
    protected virtual void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialVelocity;
    }
    protected override void Start()
    {
        GameEvents.current.PlaySound(firingSound);

        aoeDamage = GetComponent<AOEDamage>();

        var colliders = GetComponents<CircleCollider2D>();
        proximityTrigger = colliders[0];

        grenadeCollider = GetComponent<BoxCollider2D>();

        currentTimer = basicTimer;
    }

    protected virtual void Update()
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
                PlayAnimation();
            }
        }

        if (currentTimer <= basicTimer - 0.25f)
        {
            proximityFuseActive = true;
        }

        //Maintain the same speed even if our grenade bounces off a wall
        lastFrameVelocity = rb.velocity;
    }
    void PlayAnimation()
    {
        isFlashing = true;

        spriteAnimator.Play("Animation_Projectile_Grenade", -1, -1);
    }
    protected IEnumerator StartFlashing()
    {
        isFlashing = true;

        for (int i = 0; i < 20; i++)
        {
            spriteRenderer.material = matYellow;

            yield return new WaitForSeconds(0.1f);

            spriteRenderer.material = matDefault;

            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        //Grenade bounces off walls instead of 'dying'
        GameEvents.current.PlaySound(bouncingSound);

        Bounce(collision.contacts[0].normal);
    }

    protected virtual void Bounce(Vector2 collisionNormal)
    {
        //We use a custom bouncing script instead of using physics materials
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector2.Reflect(lastFrameVelocity.normalized, collisionNormal);

        rb.velocity = direction * Mathf.Max(speed, minVelocity);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //If an enemy is touching our grenade's proximity trigger, we shorten the explosion timer (if the proximityFuseActive is true)
        if (collision.IsTouching(proximityTrigger) && collision.tag == "Enemy" && proximityFuseActive)
        {
            if (currentTimer > proximityTimer)
            {
                currentTimer = proximityTimer;
            }
        }

        if (collision.IsTouching(grenadeCollider) && collision.tag == "Enemy" && proximityFuseActive)
        {
            Die();
        }
    }

    public override void Die()
    {
        if (aoeEffect != null)
        {
            Instantiate(aoeEffect, transform.position, Quaternion.identity);
        }

        //Create explosion and call AOE Damage
        if (aoeDamage != null)
        {
            aoeDamage.Explode();
        }

        Destroy(gameObject);
    }
}
