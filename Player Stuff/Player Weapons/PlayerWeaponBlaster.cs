using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWeaponBlaster : PlayerWeaponGeneric
{
    public GameObject projectilePrefab;
    public string pooledProjectileTag;

    public float fireRate = 1f;
    protected float fireRateTimer = 0f;

    public float projectileForce = 5f;

    //random min and max modifier for inaccuracy.
    public float recoilDefault;
    public float recoilMax;
    public float recoilPerShot;   //Recoil added per shot
    internal float recoilCurrent;

    //gunpoint cycling
    protected int cycle = 0;
    protected int cycleMax = 1;

    public Action<int> TurretFiredShot;

    protected override void Awake()
    {
        base.Awake();

        cycleMax = gunpoint.Length - 1;

        ResetWeaponRecoil();
    }

    void OnEnable()
    {
        fireRateTimer = fireRate;

        playerWeaponAmmo.UpdateAmmo(ammoIndex, 0);

        GameEvents.current.PlaySound(soundWeaponActivation);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
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

                Shoot();
            }

            if (recoilCurrent > 0)
            {
                recoilCurrent -= Time.deltaTime;
            }
        }
    }

    protected void Shoot()
    {
        if (projectilePrefab != null)
        {
            //Activate Particle Effects
            if (particleEffect_Gunpoint[cycle] != null)
            {
                particleEffect_Gunpoint[cycle].Play();
            }

            /*if (weaponBarrelAnimator[cycle])
            {
                weaponBarrelAnimator[cycle].Animation_Shooting();
            }*/

            if (playerWeaponSelector)
            {
                playerWeaponSelector.WeaponSwitchDelay();
            }

            //Add some random projectile rotation depending on the current recoil of the weapon
            float randomRotation = UnityEngine.Random.Range(-recoilCurrent, recoilCurrent);

            GameObject bullet = ObjectPooler.sharedInstance.GetPooledObject(pooledProjectileTag);

            if (bullet != null)
            {
                bullet.transform.position = gunpoint[cycle].position;
                bullet.transform.rotation = gunpoint[cycle].rotation;
                bullet.SetActive(true);
            }

            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

            //Add the random rotation to the projectile
            bullet.transform.Rotate(0f, 0f, randomRotation);

            if (bulletRB != null)
            {
                bulletRB.AddForce(bullet.transform.up * projectileForce, ForceMode2D.Impulse);
            }

            //Add recoil to current weapon
            if (recoilCurrent < recoilMax)
            {
                recoilCurrent += recoilPerShot;

                //cap recoil to max
                if (recoilCurrent > recoilMax)
                {
                    recoilCurrent = recoilMax;
                }
            }


            //reset the fire rate
            fireRateTimer = fireRate;

            cycle++;

            if (cycle > cycleMax)
                cycle = 0;

            playerWeaponAmmo.UpdateAmmo(ammoIndex, -1);
            //weaponUI.UpdateUI(playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent);
        }
    }

    void ResetWeaponRecoil()
    {
        recoilCurrent = recoilDefault;
    }
}
