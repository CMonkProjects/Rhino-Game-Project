using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponLaser : PlayerWeaponGeneric
{
    //weapon stuff
    public float damage = 1;
    float tracerLength = 4f;
    [SerializeField] LayerMask layerMask;  //layerMask is what objects we want our raycast to hit (in this case enemies and obstacles)

    public LineRenderer lineRenderer;

    public float fireRate = 1f;
    protected float fireRateTimer = 0f;

    [SerializeField] protected GameObject particleEffectGunpoint;
    [SerializeField] protected GameObject particleEffectHit;

    protected ParticleSystem ps_gunpoint;
    protected ParticleSystem ps_hit;

    public float chargeFull;
    float chargeCurrent;

    public bool isShootingLaser = false;

    protected override void Awake()
    {
        base.Awake();

        LineRenderer line = Instantiate(lineRenderer);
        lineRenderer = line;

        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;

        lineRenderer.enabled = false;

        if (particleEffectHit != null && particleEffectGunpoint != null)
        {
            particleEffectHit.SetActive(false);
            particleEffectGunpoint.SetActive(false);
            ps_gunpoint = particleEffectGunpoint.GetComponent<ParticleSystem>();
            ps_hit = particleEffectHit.GetComponent<ParticleSystem>();
        }

        UpdateChargeBar(playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent, playerWeaponAmmo.ammoList[ammoIndex].ammoMax);
    }

    void Start()
    {
        UpdateChargeBar(playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent, playerWeaponAmmo.ammoList[ammoIndex].ammoMax);
    }

    void OnEnable()
    {
        fireRateTimer = fireRate;
        UpdateChargeBar(playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent, playerWeaponAmmo.ammoList[ammoIndex].ammoMax);
        DisableLaser();

        GameEvents.current.PlaySound(soundWeaponActivation);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            UpdateChargeBar(playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent, playerWeaponAmmo.ammoList[ammoIndex].ammoMax);

            if (fireRateTimer > 0)
            {
                fireRateTimer -= Time.deltaTime;
            }

            if (Input.GetButton("Fire1"))
            {
                if (fireRateTimer > 0)
                {
                    return;
                }

                if (playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent <= 0)
                {
                    return;
                }

                if (chargeCurrent < chargeFull && !isShootingLaser)
                {
                    chargeCurrent += Time.deltaTime;
                    return;
                }

                if (playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent > 0 && chargeCurrent >= chargeFull && !isShootingLaser)
                {
                    StartCoroutine(ShootLaser());
                }
            }
        }
    }

    IEnumerator ShootLaser()
    {
        isShootingLaser = true;

        //Play sound effect
        GameEvents.current.PlaySound(soundWeaponFiring);

        //Activate Particle Effects
        if (particleEffectHit != null && particleEffectGunpoint != null)
        {
            particleEffectHit.SetActive(true);
            particleEffectGunpoint.SetActive(true);
            ps_gunpoint.Play();
            ps_hit.Play();
        }

        if (playerWeaponSelector)
        {
            playerWeaponSelector.WeaponSwitchDelay();
        }

        //Shoot laser
        while (playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent > 0)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                DisableLaser();
                ReloadWeapon();
                yield break;
            }

            //Shoot out the raycast
            RaycastHit2D hit = Physics2D.Raycast(gunpoint[0].position, gunpoint[0].up, tracerLength, layerMask);

            //If our raycast hits something
            if (hit)
            {
                EnemyHealth enemyHealth;
                enemyHealth = hit.transform.GetComponent<EnemyHealth>();

                /*if (enemyHealth != null)
                {
                    if (enemyHealth.damageOverTime == false)
                    {
                        enemyHealth.TakeDamageOverTime(damage);
                    }
                }*/

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamageOverTime(damage);
                }

                //For environment destructables
                DestructableHealth destructableHealth = hit.transform.GetComponent<DestructableHealth>();

                if (destructableHealth)
                {
                    destructableHealth.TakeDamageOverTime(damage);
                }

                //Put the impact effect at the place we hit
                particleEffectHit.transform.position = hit.point;
                particleEffectHit.transform.rotation = gunpoint[0].transform.rotation;

                //draw the line representing the laser
                lineRenderer.SetPosition(0, gunpoint[0].position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                //Enable two particle emitters, one from the gun point and the other at the hit point
                particleEffectHit.transform.position = gunpoint[0].position + gunpoint[0].up * tracerLength;
                particleEffectHit.transform.rotation = gunpoint[0].rotation;

                //draw a line representing the laser, if we don't hit anything
                //we just draw a long line out in the direction we're shooting
                lineRenderer.SetPosition(0, gunpoint[0].position);
                lineRenderer.SetPosition(1, gunpoint[0].position + gunpoint[0].up * tracerLength);
            }

            //draw the line renderer representing the laser
            lineRenderer.enabled = true;
            ps_gunpoint.transform.position = gunpoint[0].position;
            ps_gunpoint.transform.rotation = gunpoint[0].rotation;
            lineRenderer.sortingOrder = 0;

            playerWeaponAmmo.UpdateAmmo(ammoIndex, -Time.deltaTime);

            //Wait here for next frame
            yield return null;
        }

        DisableLaser();
        ReloadWeapon();
    }

    //Update Charge Bar
    void UpdateChargeBar(float _currentCharge, float _fullyCharged)
    {
        //Update the weapon's charge bar
        weaponUI.UpdateBar(_currentCharge, _fullyCharged);
    }

    void DisableLaser()
    {
        if (lineRenderer)
        {
            lineRenderer.enabled = false;
        }

        if (ps_gunpoint)
        {
            ps_gunpoint.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (ps_hit)
        {
            ps_hit.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        //Stop sound effect
        GameEvents.current.StopSound(soundWeaponFiring);

        UpdateChargeBar(playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent, playerWeaponAmmo.ammoList[ammoIndex].ammoMax);

        //player can charge laser again
        isShootingLaser = false;
    }

    //Reload Weapon
    void ReloadWeapon()
    {
        fireRateTimer = fireRate;
        isShootingLaser = false;
        chargeCurrent = 0;
    }

    void OnDisable()
    {
        DisableLaser();
    }
}
