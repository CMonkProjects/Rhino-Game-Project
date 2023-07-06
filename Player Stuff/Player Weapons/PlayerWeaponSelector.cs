using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSelector : MonoBehaviour
{
    public List<GameObject> weaponList;

    public int currentWeapon = 0;

    [SerializeField] bool canSwitchWeapon = true;   //is disabled for weapons that need to charge up before firing
    [SerializeField] float weaponSwitchDelay;

    PlayerWeaponAmmo playerWeaponAmmo;

    void Awake()
    {
        playerWeaponAmmo = GetComponent<PlayerWeaponAmmo>();
    }

    void Update()
    {

        int previousSelectedWeapon = currentWeapon;

        if (canSwitchWeapon)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                int newWeapon = NextWeapon();

                SelectWeapon(newWeapon);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                int newWeapon = PreviousWeapon();

                if (newWeapon != previousSelectedWeapon)
                {
                    SelectWeapon(newWeapon);
                }
            }
        }
    }

    public int NextWeapon()
    {
        int i = currentWeapon;

        do
        {
            i++;

            if (i > weaponList.Count - 1) i = 0;

            if (playerWeaponAmmo.ammoList[i].ammoCurrent > 0f) break;
        }
        while (i != currentWeapon);
        
        return i;
    }

    public int PreviousWeapon()
    {
        int i = currentWeapon;

        do
        {
            i--;

            if (i < 0) i = weaponList.Count - 1;

            if (playerWeaponAmmo.ammoList[i].ammoCurrent > 0f) break;
        }
        while (i != currentWeapon);

        return i;
    }

    public void SelectWeapon(int _selectedWeaponIndex)
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (i == _selectedWeaponIndex)
            {
                weaponList[i].gameObject.SetActive(true);
                currentWeapon = i;
            }
            else
            {
                weaponList[i].gameObject.SetActive(false);
            }
        }

        WeaponSwitchDelay();
    }

    //Weapon switch delay function
    public void WeaponSwitchDelay()
    {
        StartCoroutine(SwitchWeaponDelay());
    }

    //disables canSwitchWeapon bool for a second
    IEnumerator SwitchWeaponDelay()
    {
        canSwitchWeapon = false;

        yield return new WaitForSeconds(weaponSwitchDelay);

        canSwitchWeapon = true;
    }
    
}
