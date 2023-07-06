using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponMissile : PlayerWeaponGeneric
{
    public GameObject projectilePrefab;

    [SerializeField] Transform mouseTarget;
    [SerializeField] Transform currentTarget;

    public float fireRate = 1f;
    protected float fireRateTimer = 0f;

    //gunpoint cycling
    protected int cycle = 0;
    protected int cycleMax = 1;

    protected override void Awake()
    {
        base.Awake();

        cycleMax = gunpoint.Length - 1;
    }

    void OnEnable()
    {
        fireRateTimer = fireRate;

        ActivateMouseTarget(true);

        playerWeaponAmmo.UpdateAmmo(ammoIndex, 0);

        GameEvents.current.PlaySound(soundWeaponActivation);
    }

    void OnDisable()
    {
        ActivateMouseTarget(false);
    }

    void ActivateMouseTarget(bool _activateTarget)
    {
        if (mouseTarget != null)
        {
            mouseTarget.gameObject.SetActive(_activateTarget);

            if (_activateTarget)
            {
                currentTarget = mouseTarget;
            }
        }
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
        }
    }

    protected void Shoot()
    {
        if (projectilePrefab != null)
        {

            GameObject missile = Instantiate(projectilePrefab, gunpoint[cycle].position, gunpoint[cycle].rotation);
            HomingProjectileBehavior playerMissileBehavior = missile.GetComponent<HomingProjectileBehavior>();

            //Activate Particle Effects
            if (particleEffect_Gunpoint[cycle] != null)
            {
                particleEffect_Gunpoint[cycle].Play();
            }

            if (playerWeaponSelector)
            {
                playerWeaponSelector.WeaponSwitchDelay();
            }

            if (playerMissileBehavior && currentTarget != null)
            {
                playerMissileBehavior.SetTarget(currentTarget);
            }

            fireRateTimer = fireRate;
            playerWeaponAmmo.UpdateAmmo(ammoIndex, -1);

            cycle++;

            if (cycle > cycleMax)
                cycle = 0;
        }
    }
}
