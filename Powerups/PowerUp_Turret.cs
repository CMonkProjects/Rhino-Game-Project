using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp_Turret : PowerUp
{
    public int turretIndex;

    public int blasterAmmo;

    protected override void PowerupPayload()
    {
        base.PowerupPayload();

        GameObject turretController = GameObject.Find("PlayerTurretController");

        GameObject blasterTurret = GameObject.Find("PlayerRobotTurretBlaster");

        if (turretController != null)
        {
            //turretController.GetComponent<PlayerWeaponSelector>().SelectWeapon(turretIndex);

            /*if (blasterTurret != null)
            {
                blasterTurret.GetComponent<TurretLimitedAmmo>().GiveAmmo(blasterAmmo);
            }*/
        }
    }
}
