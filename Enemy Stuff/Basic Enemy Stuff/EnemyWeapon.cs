using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class EnemyWeapon
{
    [Header("Enemy weapon type")]
    public GameObject projectilePrefab;

    public float projectileForce;
    public int numberOfSalvos;      //A salvo is a group of shots
    public int shotsPerSalvo;   
    public float fireRate;          //wait time per shot in a salvo
    public float waitBetweenSalvo; //wait time between salvos
    public float waitToFire;        //wait time before shooting period

    //random min and max modifier for inaccuracy.
    public bool addRecoilPerShot = false;
    public float recoilStarting;
    public float recoilMax;
    public float recoilPerShot;   //Every time an enemy shoots, the total recoil adds up making the firing spread more inaccurate
    internal float recoilCurrent;

    public bool weaponIsHoming;     //needs the player target to fire

    //References to the gunpoints on the enemy turret it'll use
    public Transform[] gunpoint = new Transform[0];
    public WeaponAnimator[] weaponAnimator;
    //Particle effects
    public ParticleSystem[] particleEffect_Gunpoint = new ParticleSystem[0];

    internal void AdjustProjectile(GameObject _projectile, int _cycle)
    {
        float randomRotation = UnityEngine.Random.Range(-recoilCurrent, recoilCurrent);

        //GameObject bullet = Instantiate(projectilePrefab,
        GameObject bullet = _projectile;
        
        if (!bullet)
        {
            return;
        }

        //Add some random rotation to the instantiated projectile depending on the recoil
        bullet.transform.Rotate(0f, 0f, randomRotation);

        //Play the particle effect if we have one
        if (particleEffect_Gunpoint.Length > 0 && particleEffect_Gunpoint[_cycle] != null)
        {
            particleEffect_Gunpoint[_cycle].Play();
        }
        //Play shooting animation if we have one
        if (weaponAnimator.Length > 0 && weaponAnimator[_cycle] != null)
        {
            weaponAnimator[_cycle].Animation_Shooting();
        }

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        if (bulletRB)
        {
            bulletRB.AddForce(bullet.transform.up * projectileForce, ForceMode2D.Impulse);
        }
        
        //Add any recoil to current weapon
        if (addRecoilPerShot)
        {
            if (recoilCurrent < recoilMax)
            {
                recoilCurrent += recoilPerShot;

                //cap recoil to max
                if (recoilCurrent > recoilMax)
                {
                    recoilCurrent = recoilMax;
                }
            }
        }
    }

    internal void ResetRecoil()
    {
        recoilCurrent = recoilStarting;
    }
}
