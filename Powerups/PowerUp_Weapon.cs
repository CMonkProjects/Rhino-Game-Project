using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp_Weapon : PowerUp
{
    public int weaponIndex;
    public int ammoCount = 1;

    protected override void CheckConditions()
    {
        if (playerWeaponAmmo.ammoList[weaponIndex].ammoCurrent >= playerWeaponAmmo.ammoList[weaponIndex].ammoMax)
        {
            conditionResult = false;
            Debug.Log("Player already at full ammo for " + playerWeaponAmmo.ammoList[weaponIndex].weaponName);
        }
        else
        {
            conditionResult = true;
        }
    }

    protected override void PowerupPayload()
    {
        base.PowerupPayload();

        //GameObject weaponHolder = GameObject.Find("PlayerTurretController");

        if (playerWeaponAmmo != null)
        {
            playerWeaponAmmo.UpdateAmmo(weaponIndex, ammoCount);
        }
    }
}
