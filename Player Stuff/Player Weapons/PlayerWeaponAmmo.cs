using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponAmmo
{
    public WeaponUI weaponUI;
    public string weaponName;
    public float ammoCurrent;
    public int ammoMax;
}

public class PlayerWeaponAmmo : MonoBehaviour
{
    public List<WeaponAmmo> ammoList = new List<WeaponAmmo>();

    public void UpdateAmmo(int _weaponIndex, float _ammo)
    {
        ammoList[_weaponIndex].ammoCurrent += _ammo;

        if (ammoList[_weaponIndex].ammoCurrent > ammoList[_weaponIndex].ammoMax)
        {
            ammoList[_weaponIndex].ammoCurrent = ammoList[_weaponIndex].ammoMax;
        }

        if (ammoList[_weaponIndex].weaponUI.displayAmmo)
        {
            ammoList[_weaponIndex].weaponUI.UpdateUI(ammoList[_weaponIndex].ammoCurrent);
        }
        
        if (ammoList[_weaponIndex].weaponUI.hasWeaponBar)
        {
            ammoList[_weaponIndex].weaponUI.UpdateBar(ammoList[_weaponIndex].ammoCurrent, ammoList[_weaponIndex].ammoMax);
        }
    }
}
