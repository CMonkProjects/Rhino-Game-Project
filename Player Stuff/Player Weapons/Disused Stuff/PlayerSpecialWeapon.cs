using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialWeapon : MonoBehaviour
{
    public Transform[] gunpoint = new Transform[0];
    public GameObject bulletPrefab;

    public string weaponName = "";
    public string soundEffect = "";
    public int ammo = 1;
    public float bulletForce = 20f;
    public bool projectileSelfPropelled = false;

    public float fireRate = 1f;
    protected float currentFireRate = 0f;

    public SpecialWeaponSwitching specialWeaponSwitching;
    public WeaponUI weaponUI;

    public int specialWeaponIndex = 0;
    protected bool canShoot = false;
    public float readyToShoot = 1f;
    [SerializeField] protected bool oneProjecileAtOneTime = false;
    protected GameObject activeProjectile;

    protected WeaponAnimator weaponSpriteAnimator;
    [SerializeField] protected WeaponAnimator[] weaponBarrelAnimator = new WeaponAnimator[0];

    //Particle effects
    [SerializeField] protected ParticleSystem[] particleEffect_Gunpoint = new ParticleSystem[0];

    protected int cycle = 0;
    protected int cycleMax;

    protected virtual void Awake()
    {
        weaponSpriteAnimator = GetComponent<WeaponAnimator>();

        cycleMax = gunpoint.Length - 1;
    }
    protected virtual void Start()
    {
        specialWeaponSwitching = GetComponentInParent<SpecialWeaponSwitching>();
        weaponUI = GetComponent<WeaponUI>();
    }

    protected virtual void OnEnable()
    {
        //Wait a second before we can shoot
        StartCoroutine(ReadyWeaponToFire());
        GameEvents.current.PlaySound(soundEffect);
    }

    protected virtual void OnDisable()
    {
        canShoot = false;
    }

    protected virtual IEnumerator ReadyWeaponToFire()
    {
        yield return new WaitForSeconds(readyToShoot);

        canShoot = true;
    }

    protected virtual void Update()
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

                Shoot();
            }
        }
    }

    protected virtual void DisableWeapon()
    {
        Destroy(gameObject);
        specialWeaponSwitching.DisabledWeapon();
    }

    protected virtual void Shoot()
    {
        if (oneProjecileAtOneTime)
        {
            if (activeProjectile != null)
            {
                return;
            }
        }

        if (bulletPrefab != null)
        {
            //Play the weapon shooting animation
            if (weaponSpriteAnimator)
            {
                weaponSpriteAnimator.Animation_Shooting();
            }

            if (weaponBarrelAnimator[cycle])
            {
                weaponBarrelAnimator[cycle].Animation_Shooting();
            }

            if (particleEffect_Gunpoint[cycle])
            {
                particleEffect_Gunpoint[cycle].Play();
            }

            GameObject bullet = Instantiate(bulletPrefab, gunpoint[cycle].position, gunpoint[cycle].rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            activeProjectile = bullet;

            if (!projectileSelfPropelled)
            {
                bulletRB.AddForce(gunpoint[cycle].up * bulletForce, ForceMode2D.Impulse);
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

    public virtual void AddAmmo(int _newAmmo)
    {
        ammo += _newAmmo;

        weaponUI.UpdateUI(ammo);
    }
}
