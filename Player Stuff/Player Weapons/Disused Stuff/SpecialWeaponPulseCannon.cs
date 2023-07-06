using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWeaponPulseCannon : PlayerSpecialWeapon
{
    //random min and max modifier for inaccuracy.
    public float recoilDefault;
    public float recoilMax;
    public float recoilPerShot;   //Recoil added per shot
    [SerializeField] internal float recoilCurrent;

    public string pooledBulletTag;

    protected override void Awake()
    {
        base.Awake();

        ResetWeaponRecoil();
    }

    protected override void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (currentFireRate > 0)
            {
                currentFireRate -= Time.deltaTime;
                return;
            }

            if (Input.GetButton("Fire1") && canShoot)
            {
                if (ammo <= 0)
                {
                    DisableWeapon();
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

    protected override void Shoot()
    {
        if (bulletPrefab != null)
        {
            //Activate Particle Effects
            if (particleEffect_Gunpoint[cycle] != null)
            {
                particleEffect_Gunpoint[cycle].Play();
            }

            //Play the weapon shooting animation
            if (weaponSpriteAnimator)
            {
                weaponSpriteAnimator.Animation_Shooting();
            }

            if (weaponBarrelAnimator[cycle])
            {
                weaponBarrelAnimator[cycle].Animation_Shooting();
            }

            //Add some random projectile rotation depending on the current recoil of the weapon
            float randomRotation = Random.Range(-recoilCurrent, recoilCurrent);

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

            ammo--;

            if (ammo <= 0)
            {
                DisableWeapon();
                return;
            }

            currentFireRate = fireRate;

            weaponUI.UpdateUI(ammo);

            cycle++;

            if (cycle > cycleMax)
                cycle = 0;
        }
    }

    void ResetWeaponRecoil()
    {
        recoilCurrent = recoilDefault;
    }
}
