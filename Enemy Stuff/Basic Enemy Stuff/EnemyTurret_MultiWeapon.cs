using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret_MultiWeapon : EnemyTurret
{
    bool autoFire = true;

    internal delegate void DoneShooting(int weaponIndex);
    internal DoneShooting doneShooting;

    protected override void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            //If turret is inactive then don't move or shoot (only happens when enemy is being destroyed)
            if (!turretActive)
            {
                return;
            }

            if (currentTarget != null)
            {
                //Direction to rotate towards target
                float angle = Mathf.Atan2(currentTarget.position.y - transform.position.y, currentTarget.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

                //Rotate the turret towards the target
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

                //Turret firing normally on its own (switch off to use the secondary attack instead)
                if (!autoFire)
                    return;

                if (!canAttack)
                    return;

                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                    return;
                }

                if (targetInViewCone && hasLineOfSight && canAttack)
                {
                    StartCoroutine(StartShooting());
                }
            }
            else
            {
                //Rotate back towards default rotation
                if (transform.parent == null)
                {
                    return;
                }

                Quaternion defaultRotation = transform.parent.localRotation;

                transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
    protected internal override IEnumerator StartShooting()
    {
        canAttack = false;

        //Wait for this coroutine to finish before moving on
        yield return shootingCoroutine = StartCoroutine(Shooting(currentWeaponIndex));

        yield return new WaitForEndOfFrame();

        ResetWeapon();

        if (doneShooting != null)
        {
            doneShooting(currentWeaponIndex);
        }
    }

    protected internal void SwitchWeapon(int _newWeaponIndex)
    {
        currentWeaponIndex = _newWeaponIndex;

        ResetWeapon();
    }

    protected internal void SetAutoFire(bool _autoFire)
    {
        autoFire = _autoFire;
    }
}