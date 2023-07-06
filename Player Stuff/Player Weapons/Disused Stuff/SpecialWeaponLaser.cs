using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWeaponLaser : PlayerSpecialWeapon
{
    public string soundEffectShooting;

    public float damage = 1;

    public float batteryFull;
    float currentBattery;

    [SerializeField] protected float tracerLength = 1f;
    [SerializeField] protected LayerMask layerMask;  //layerMask is what objects we want our raycast to hit (enemies and obstacles)
    [SerializeField] protected string TreeTag = "Tree";

    public LineRenderer lineRenderer;
    public float lineStartWidth = 0.03f;
    public float lineEndWidth = 0.03f;

    public float chargeLevel;
    public bool canShootLaser = false;
    [SerializeField] float chargeRate = 1.5f;

    public string laserLoopSound = "LaserLoopShoot";
    public string laserChargeSound = "LaserCharge";

    [SerializeField] protected GameObject particleEffectGunpoint;
    [SerializeField] protected GameObject particleEffectHit;

    protected ParticleSystem ps_gunpoint;
    protected ParticleSystem ps_hit;

    protected virtual void Awake()
    {
        LineRenderer line = Instantiate(lineRenderer);
        lineRenderer = line;

        lineRenderer.startWidth = lineStartWidth;
        lineRenderer.endWidth = lineEndWidth;

        lineRenderer.enabled = false;

        if (particleEffectHit != null && particleEffectGunpoint != null)
        {
            particleEffectHit.SetActive(false);
            particleEffectGunpoint.SetActive(false);
            ps_gunpoint = particleEffectGunpoint.GetComponent<ParticleSystem>();
            ps_hit = particleEffectHit.GetComponent<ParticleSystem>();
        }
    }

    protected override void Start()
    {
        base.Start();

        UpdateChargeBar(chargeLevel, 1f);
    }

    protected override void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (Input.GetButtonDown("Fire2") && canShoot && !canShootLaser)
            {
                if (ammo <= 0)
                {
                    DisableWeapon();
                    return;
                }

                StartCoroutine(ChargeWeapon());
            }

            if (Input.GetButtonUp("Fire2") && canShoot && canShootLaser && chargeLevel == 1)
            {
                StartCoroutine(ShootLaser());
            }
        }
    }

    protected virtual IEnumerator ChargeWeapon()
    {
        GameEvents.current.PlaySound(laserChargeSound);
        //Enable the charging particle emitter

        while (chargeLevel < 1)
        {
            //Track the charge level
            chargeLevel += Time.deltaTime / chargeRate; //charge for 2 seconds

            //If player lets go of the charge button
            if (Input.GetButtonUp("Fire2"))
            {
                chargeLevel = 0;
                UpdateChargeBar(chargeLevel, 1);
                GameEvents.current.StopSound(laserChargeSound);
                //Disable the charging particle emitter
                yield break;
            }

            UpdateChargeBar(chargeLevel, 1);
            //Wait here for next frame
            yield return null;
        }

        canShootLaser = true;
        //Clamp chargeLevel to full
        chargeLevel = 1f;
    }

    protected override void Shoot()
    {
        //Nothing Here
    }

    IEnumerator ShootLaser()
    {
        currentBattery = batteryFull;
        chargeLevel = 0;
        UpdateChargeBar(chargeLevel, 1);

        //Don't allow the player to switch weapons while firing laser
        specialWeaponSwitching.CanSwitchWeapon(false);

        //Play sound effect
        GameEvents.current.PlaySound(laserLoopSound);

        //Activate Particle Effects
        if (particleEffectHit != null && particleEffectGunpoint != null)
        {
            particleEffectHit.SetActive(true);
            particleEffectGunpoint.SetActive(true);
            ps_gunpoint.Play();
            ps_hit.Play();
        }

        while (currentBattery > 0f)
        {
            UpdateChargeBar(currentBattery, batteryFull);
            //Shoot out the raycast
            RaycastHit2D hit = Physics2D.Raycast(gunpoint[cycle].position, gunpoint[cycle].up, tracerLength, layerMask);

            //If our raycast hits something
            if (hit)
            {
                EnemyHealth enemyHealth;
                enemyHealth = hit.transform.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    //if (enemyHealth.damageOverTime == false)
                    //{
                        //enemyHealth.TakeDamageOverTime(damage);
                    //}
                }

                DestructableHealth healthTree = hit.transform.GetComponent<DestructableHealth>();

                if (healthTree)
                {
                    healthTree.TakeDamage(damage);
                }

                //Create the impact effect at the place we hit
                //Quaternion Identity is just a fancy way of saying 'no rotation'

                //Enable two particle emitters, one from the gun point and the other at the hit point
                particleEffectHit.transform.position = hit.point;
                particleEffectHit.transform.localEulerAngles = new Vector3(0, 0, gunpoint[cycle].rotation.z - 90);

                //draw the line representing the laser
                lineRenderer.SetPosition(0, gunpoint[cycle].position);
                lineRenderer.SetPosition(1, hit.point);
                
            }
            else
            {
                //Enable two particle emitters, one from the gun point and the other at the hit point
                particleEffectHit.transform.position = gunpoint[cycle].position + gunpoint[cycle].up * tracerLength;
                particleEffectHit.transform.rotation = gunpoint[cycle].rotation;

                //draw a line representing the laser, if we don't hit anything
                //we just draw a long line out in the direction we're shooting
                lineRenderer.SetPosition(0, gunpoint[cycle].position);
                lineRenderer.SetPosition(1, gunpoint[cycle].position + gunpoint[cycle].up * tracerLength);
            }

            //draw the line renderer representing the laser
            lineRenderer.enabled = true;
            ps_gunpoint.transform.position = gunpoint[cycle].position;
            ps_gunpoint.transform.rotation = gunpoint[cycle].rotation;
            lineRenderer.sortingOrder = 0;

            currentBattery -= Time.deltaTime;
            //Wait till next frame
            yield return null;
        }

        DisableLaser();
        ReloadWeapon();
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
        GameEvents.current.StopSound(laserLoopSound);
        //Stop charging sound effect (If player switched weapon while charging laser)
        GameEvents.current.StopSound(laserChargeSound);

        currentBattery = 0;
        chargeLevel = 0;
        UpdateChargeBar(chargeLevel, 1);
        //player can charge laser again
        canShootLaser = false;
    }

    protected virtual void UpdateChargeBar(float _currentCharge, float _fullyCharged)
    {
        //Update the weapon's charge bar
        weaponUI.UpdateBar(_currentCharge, _fullyCharged);
    }

    protected virtual void ReloadWeapon()
    {
        ammo--;
        canShootLaser = false;

        weaponUI.UpdateUI(ammo);
        //Allow player to switch weapons again
        specialWeaponSwitching.CanSwitchWeapon(true);
    }

    protected virtual void OnDestroy()
    {
        if (lineRenderer != null)
            Destroy(lineRenderer);

        //Stop sound effect
        GameEvents.current.StopSound(laserLoopSound);
    }

    protected override void OnDisable()
    {
        DisableLaser();
    }
}