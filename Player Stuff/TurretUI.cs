using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretUI : WeaponUI
{
    internal override void Start()
    {
        //turretLimitedAmmo = GetComponent<TurretLimitedAmmo>();
        //weaponName = turretLimitedAmmo.turretName;
        CreateUI();
    }

    internal override void CreateUI()
    {
        gameUI = GameObject.Find("Game_UI");

        if (gameUI != null)
        {
            weaponGUI = Instantiate(weaponGUIPrefab, gameUI.transform);
            weaponNameText = weaponGUI.transform.Find("TurretNameText").GetComponent<Text>();
            weaponAmmoText = weaponGUI.transform.Find("TurretAmmoText").GetComponent<Text>();
            weaponImage = weaponGUI.transform.Find("TurretImage").GetComponent<Image>();

            if (hasWeaponBar)
            {
                weaponBar = weaponGUI.transform.Find("TurretBar").GetComponent<Slider>();
            }

            if (weaponNameText != null)
            {
                weaponNameText.text = weaponName;
            }

            if (weaponAmmoText != null && displayAmmo)
            {
                weaponAmmoText.text = ammoString + ammo;
            }
            else
            {
                weaponAmmoText.text = "";
            }

            if (weaponImage != null)
            {
                weaponImage.sprite = weaponSprite;
            }
        }
    }
}
