using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponMissile_Old : PlayerWeaponGeneric
{
    public GameObject projectilePrefab;

    [SerializeField] bool oneProjectileAtOneTime;
    GameObject activeProjectile;

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
        if (oneProjectileAtOneTime)
        {
            if (activeProjectile != null)
            {
                return;
            }
        }

        if (projectilePrefab != null)
        {
            GameObject missile = Instantiate(projectilePrefab, gunpoint[cycle].position, gunpoint[cycle].rotation);
            activeProjectile = missile;
            HomingMissileBehavior_Old homingMissileBehavior = missile.GetComponent<HomingMissileBehavior_Old>();

            if (homingMissileBehavior != null && currentTarget != null)
            {
                homingMissileBehavior.target = currentTarget;
            }

            fireRateTimer = fireRate;
            playerWeaponAmmo.UpdateAmmo(ammoIndex, -1);
        }
    }
}