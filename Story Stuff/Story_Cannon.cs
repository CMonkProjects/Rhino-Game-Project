using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Story_Cannon : MonoBehaviour
{
    //superweapon charge
    [SerializeField] internal float superweaponChargeRate;
    [SerializeField] internal float superweaponChargeTime;
    [SerializeField] internal float superweaponCharge;

    [SerializeField] float timer;

    //Projectile
    [SerializeField] GameObject superweaponProjectile;
    [SerializeField] float projectileForce;
    [SerializeField] Transform projectileLaunchPoint;

    [SerializeField] string objectiveTargetID;  //just input this manually

    //Particle effect
    [SerializeField] GameObject particleEffect;

    //Weapon Animation
    public WeaponAnimator[] weaponAnimator;

    void OnEnable()
    {
        timer = 0f;
        superweaponCharge = 0f;

        Invoke("SubscribeToEvents", 0.1f);
    }

    void OnDisable()
    {
        GameEvents.current.onGeneratorDestroyed -= GeneratorLost;
        GameEvents.current.onPrimaryGeneratorOnline -= PrimaryGeneratorOnline;
        GameEvents.current.onPrimaryGeneratorDestroyed -= PrimaryGeneratorDestroyed;
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onGeneratorDestroyed += GeneratorLost;
        GameEvents.current.onPrimaryGeneratorOnline += PrimaryGeneratorOnline;
        GameEvents.current.onPrimaryGeneratorDestroyed += PrimaryGeneratorDestroyed;
    }

    protected void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (superweaponCharge < superweaponChargeTime)
            {
                superweaponCharge += Time.deltaTime * superweaponChargeRate;
            }
            else
            {
                WeaponFiring();
            }
        }
    }

    protected void WeaponFiring()
    {
        //weapon firing event goes here
        superweaponCharge = 0f;
        timer = 0f;

        GameObject projectile = Instantiate(superweaponProjectile,
                projectileLaunchPoint.position,
                projectileLaunchPoint.rotation);

        Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();

        if (projectileRB)
        {
            projectileRB.AddForce(projectile.transform.up * projectileForce, ForceMode2D.Impulse);
        }

        //Play the particle effect if we have one
        /*if (particleEffect != null)
        {
            particleEffect[0].Play();
        }*/

        GameObject _particleEffect = Instantiate(particleEffect,
                projectileLaunchPoint.position,
                projectileLaunchPoint.rotation);

        //play animation of weapon barrel
        if (weaponAnimator[0] != null)
        {
            weaponAnimator[0].Animation_Shooting();
        }

        GameEvents.current.SuperweaponFiring();
    }

    void GeneratorLost()
    {
        superweaponChargeRate -= 1f;

        if (superweaponChargeRate <= 0)
        {
            GameEvents.current.AllGeneratorsDestroyed();
        }
    }

    void PrimaryGeneratorOnline()
    {
        superweaponChargeRate = 1f;
    }

    void PrimaryGeneratorDestroyed()
    {
        superweaponChargeRate = 0f;
    }
}
