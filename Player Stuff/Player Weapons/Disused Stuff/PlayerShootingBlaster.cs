using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShootingBlaster : PlayerShootingRaycast
{
    public float bulletForce = 20f;
    //random min and max modifier for inaccuracy.
    public float recoilDefault;
    public float recoilMax;
    public float recoilPerShot;   //Recoil added per shot
    [SerializeField] internal float recoilCurrent;

    public GameObject bulletPrefab;
    public string pooledBulletTag;

    public Action<int> TurretFiredShot;
    void Awake()
    {
        cycleMax = gunpoint.Length - 1;

        ResetWeaponRecoil();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (Input.GetButton("Fire1"))
            {
                if (readyToFire > 0)
                {
                    readyToFire -= Time.deltaTime;
                    return;
                }

                /*if (turretAmmo)
                {
                    if (turretAmmo.ammo < 1)
                    {
                        return;
                    }
                }*/

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
        if (bulletPrefab != null)
        {
            //Activate Particle Effects
            if (particleEffect_Gunpoint[cycle] != null)
            {
                particleEffect_Gunpoint[cycle].Play();
            }

            if (weaponBarrelAnimator[cycle])
            {
                weaponBarrelAnimator[cycle].Animation_Shooting();
            }

            //Add some random projectile rotation depending on the current recoil of the weapon
            float randomRotation = UnityEngine.Random.Range(-recoilCurrent, recoilCurrent);

            GameObject bullet = ObjectPooler.sharedInstance.GetPooledObject(pooledBulletTag);

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
                bulletRB.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
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
            readyToFire = fireRate;

            cycle++;

            if (cycle > cycleMax)
                cycle = 0;

            if (TurretFiredShot != null)
            {
                TurretFiredShot(ammoCost);
            }
        }
    }

    void ResetWeaponRecoil()
    {
        recoilCurrent = recoilDefault;
    }
}
