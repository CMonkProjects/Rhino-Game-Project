using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWeaponRailgun : SpecialWeaponLaser
{
    WeaponAnimator specialWeaponAnimator;
    protected override void Awake()
    {
        LineRenderer line = Instantiate(lineRenderer);
        lineRenderer = line;

        lineRenderer.startWidth = lineStartWidth;
        lineRenderer.endWidth = lineEndWidth;

        lineRenderer.enabled = false;

        if (particleEffectGunpoint != null)
        {
            particleEffectGunpoint.SetActive(false);
            ps_gunpoint = particleEffectGunpoint.GetComponent<ParticleSystem>();
        }
        specialWeaponAnimator = GetComponent<WeaponAnimator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (particleEffectGunpoint)
        {
            particleEffectGunpoint.SetActive(false);
        }
    }
    // Update is called once per frame
    protected override void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
            {
                if (currentFireRate > 0)
                {
                    currentFireRate -= Time.deltaTime;
                    return;
                }

                if (Input.GetButton("Fire2") && canShoot)
                {
                    if (ammo <= 0)
                    {
                        DisableWeapon();
                        return;
                    }

                    StartCoroutine(ShootRailgun());
                }
            }
        }
    }

    IEnumerator ShootRailgun()
    {
        currentFireRate = fireRate;

        GameEvents.current.PlaySound(soundEffectShooting);

        //Shoot out the raycast
        RaycastHit2D hit = Physics2D.Raycast(gunpoint[cycle].position, gunpoint[cycle].up, tracerLength, layerMask);

        //Activate Particle Effects and sprite animations
        if (particleEffectGunpoint)
        {
            particleEffectGunpoint.SetActive(true);
            ps_gunpoint.Play();

            //Play the weapon shooting animation
            specialWeaponAnimator.Animation_Shooting();
        }

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

            //Create particle effect facing towards the special weapon's gun (so sparks fly outward away from the impact)
            Instantiate(particleEffectHit, hit.point, transform.rotation * Quaternion.Euler(0,0,-45f));

            //draw the line representing the laser
            lineRenderer.SetPosition(0, gunpoint[cycle].position);
            lineRenderer.SetPosition(1, hit.point);

        }
        else
        {
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

        //Wait a bit
        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;
        ps_gunpoint.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ReloadWeapon();
    }

    protected override void ReloadWeapon()
    {
        ammo--;

        if (ammo <= 0)
        {
            DisableWeapon();
            return;
        }

        weaponUI.UpdateUI(ammo);
    }
}
