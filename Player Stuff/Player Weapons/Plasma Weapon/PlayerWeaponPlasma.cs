using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponPlasma : PlayerWeaponGeneric
{
    public string weaponChargeSound = "";
    public string weaponFullyChargedSound = "";

    [SerializeField] Transform mouseTarget;
    [SerializeField] Transform currentTarget;

    public GameObject projectilePrefab;

    public float fireRate = 1f;
    protected float fireRateTimer = 0f;

    public float chargeFull;
    [SerializeField] float chargeCurrent = 0f;

    public ParticleSystem particleEffect_Charging;
    public ParticleSystem particleEffect_FullyCharged;

    public float projectileForce = 2f;

    protected override void Awake()
    {
        base.Awake();

        weaponUI = GetComponent<WeaponUI>();

        ReloadWeapon();
    }

    void OnEnable()
    {
        ActivateMouseTarget(true);

        ReloadWeapon();

        GameEvents.current.PlaySound(soundWeaponActivation);
    }

    void OnDisable()
    {
        ReloadWeapon();

        ActivateMouseTarget(false);
    }

    void ActivateMouseTarget(bool _activateTarget)
    {
        if (mouseTarget != null)
        {
            mouseTarget.gameObject.SetActive(_activateTarget);

            if (_activateTarget)
            {
                currentTarget = null;
            }
        }
    }

    public void NewTarget(Transform _newTarget)
    {
        currentTarget = _newTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            UpdateChargeBar(chargeCurrent, chargeFull);

            if (fireRateTimer > 0)
            {
                fireRateTimer -= Time.deltaTime;
            }

            //Hold down LMB to charge weapon
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

                //Charge weapon until full
                chargeCurrent += Time.deltaTime;
                UpdateChargeBar(chargeCurrent, chargeFull);

                if (chargeCurrent <= chargeFull)
                {
                    if (particleEffect_Charging != null && !particleEffect_Charging.isPlaying)
                    {
                        particleEffect_Charging.Play();

                        GameEvents.current.PlaySound(weaponChargeSound);
                    }

                    return;
                }

                if (chargeCurrent >= chargeFull)
                {
                    if (particleEffect_Charging != null && particleEffect_Charging.isPlaying)
                    {
                        particleEffect_Charging.Stop();

                        GameEvents.current.StopSound(weaponChargeSound);

                        GameEvents.current.PlaySound(weaponFullyChargedSound);
                    }
                }

                if (particleEffect_FullyCharged != null && !particleEffect_FullyCharged.isPlaying)
                {
                    particleEffect_FullyCharged.Play();
                }
            }

            //Let go to fire
            if (Input.GetButtonUp("Fire1"))
            {
                CheckWeaponCharge();
            }
        }
    }

    void CheckWeaponCharge()
    {
        GameEvents.current.StopSound(weaponChargeSound);

        if (chargeCurrent >= chargeFull)
        {
            Shoot();

            StopChargeParticleEffect();
        }
        else
        {
            chargeCurrent = 0f;

            StopChargeParticleEffect();
        }

        UpdateChargeBar(chargeCurrent, chargeFull);
    }

    void StopChargeParticleEffect()
    {
        if (particleEffect_Charging != null && particleEffect_Charging.isPlaying)
        {
            particleEffect_Charging.Stop();
        }

        if (particleEffect_FullyCharged != null && particleEffect_FullyCharged.isPlaying)
        {
            particleEffect_FullyCharged.Stop();
        }
    }

    //void Shoot
    void Shoot()
    {
        if (projectilePrefab != null)
        {
            //Activate Particle Effects
            if (particleEffect_Gunpoint[0] != null)
            {
                particleEffect_Gunpoint[0].Play();
            }

            if (playerWeaponSelector)
            {
                playerWeaponSelector.WeaponSwitchDelay();
            }

            GameObject projectile = Instantiate(projectilePrefab, gunpoint[0].position, gunpoint[0].rotation);
            HomingProjectileBehavior homingProjectileTarget = projectile.GetComponent<HomingProjectileBehavior>();

            if (homingProjectileTarget && currentTarget != null)
            {
                homingProjectileTarget.SetTarget(currentTarget);
            }

            Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();

            if (projectileRB != null)
            {
                projectileRB.AddForce(projectile.transform.up * projectileForce, ForceMode2D.Impulse);
            }

            playerWeaponAmmo.UpdateAmmo(ammoIndex, -1);
            ReloadWeapon();
        }
    }

    //Update Charge Bar
    void UpdateChargeBar(float _currentCharge, float _fullyCharged)
    {
        //Update the weapon's charge bar
        weaponUI.UpdateBar(_currentCharge, _fullyCharged);
    }

    //Reload Weapon
    void ReloadWeapon()
    {
        fireRateTimer = fireRate;
        chargeCurrent = 0f;
        UpdateChargeBar(chargeCurrent, chargeFull);
        playerWeaponAmmo.UpdateAmmo(ammoIndex, 0);
        GameEvents.current.StopSound(weaponChargeSound);
    }
}
