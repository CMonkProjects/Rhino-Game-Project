using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWeaponMissileLauncher : PlayerSpecialWeapon
{
    //currentTarget is designated by PlayerMissileTargeting
    [SerializeField] Transform currentTarget;
    [SerializeField] GameObject muzzleflash;
    Animator muzzleFlashAnimator;

    [SerializeField] Transform mouseTarget;

    //Special Effects
    //[SerializeField] protected GameObject particleEffectMuzzleflash;

    void Awake()
    {
        if (muzzleflash != null)
        {
            //particleEffectMuzzleflash.SetActive(false);
            //ps_muzzleflash = particleEffectMuzzleflash.GetComponent<ParticleSystem>();
            muzzleflash.SetActive(false);
            muzzleFlashAnimator = muzzleflash.GetComponent<Animator>();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        currentTarget = null;

        if (muzzleflash)
        {
            muzzleflash.SetActive(false);
        }

        ActivateTarget(true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        ActivateTarget(false);
    }

    //Set the homing missiles to follow the mouse
    protected virtual void ActivateTarget(bool _activateTarget)
    {
        if (mouseTarget != null)
        {
            mouseTarget.gameObject.SetActive(_activateTarget);
        }

        if (_activateTarget)
        {
            NewTarget(mouseTarget);
        }
    }

    public void NewTarget(Transform _newTarget)
    {
        if (_newTarget)
        {
            currentTarget = _newTarget.transform;
        }
        else currentTarget = null;
    }

    protected override void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (currentFireRate > 0 && activeProjectile == null)
            {
                currentFireRate -= Time.deltaTime;
                return;
            }

            if (ammo <= 0 && activeProjectile == null)
            {
                DisableWeapon();
                return;
            }

            if (Input.GetButton("Fire2") && canShoot && activeProjectile == null)
            {
                Shoot();
            }
        }
    }

    protected override void Shoot()
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
            GameObject missile = Instantiate(bulletPrefab, gunpoint[cycle].position, gunpoint[cycle].rotation);
            Rigidbody2D missileRB = missile.GetComponent<Rigidbody2D>();
            activeProjectile = missile;
            HomingMissileBehavior_Old homingMissileBehavior = missile.GetComponent<HomingMissileBehavior_Old>();

            //Activate Particle Effects
            if (muzzleflash != null)
            {
                //particleEffectMuzzleflash.SetActive(true);
                //ps_muzzleflash.Play();
                muzzleflash.SetActive(true);
                muzzleFlashAnimator.Play("Animation_Muzzleflash_MissileLauncher", -1, 0f);
            }

            if (homingMissileBehavior != null && currentTarget != null)
            {
                homingMissileBehavior.target = currentTarget;
            }

            if (!projectileSelfPropelled)
            {
                missileRB.AddForce(gunpoint[cycle].up * bulletForce, ForceMode2D.Impulse);
            }

            ammo--;

            currentFireRate = fireRate;

            weaponUI.UpdateUI(ammo);
        }
    }

    protected override void DisableWeapon()
    {
        if (mouseTarget != null)
        {
            Destroy(mouseTarget.gameObject);
        }

        base.DisableWeapon();
    }
}
