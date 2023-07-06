using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//This is based off Brackey's weapon switching tutorial
public class SpecialWeaponSwitching : MonoBehaviour
{
    [SerializeField] int selectedWeapon = 0;

    [SerializeField] bool canSwitchSpecialWeapon = true;

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (canSwitchSpecialWeapon)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1)
                {
                    selectedWeapon = 0;
                }
                else selectedWeapon++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= 0)
                {
                    selectedWeapon = transform.childCount - 1;
                }
                else selectedWeapon--;
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                SelectWeapon(selectedWeapon);
            }
        }
    }

    void SelectWeapon(int _selectedWeapon)
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == _selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        
            i++;
        }
    }

    //Called by Powerup Weapon pickup
    public void WeaponPickup(GameObject _newWeaponPrefab, int newAmmo)
    {
        PlayerSpecialWeapon playerSpecialWeapon;

        //Checks to see if we already have an instance of the weapon
        Transform weapon = gameObject.transform.Find(_newWeaponPrefab.name + "(Clone)");

        if (!weapon)
        {
            Debug.Log("We did not find the weapon.  Adding new weapon");
            GameObject newWeapon = Instantiate(_newWeaponPrefab, gameObject.transform);
            playerSpecialWeapon = newWeapon.GetComponent<PlayerSpecialWeapon>();
            playerSpecialWeapon.AddAmmo(newAmmo);

            selectedWeapon = transform.childCount - 1;
            SelectWeapon(selectedWeapon);
        }
        else
        {
            Debug.Log("We found the weapon.  Adding ammo");
            playerSpecialWeapon = weapon.GetComponent<PlayerSpecialWeapon>();

            playerSpecialWeapon.AddAmmo(newAmmo);
        }
    }

    public void DisabledWeapon()
    {
        Debug.Log("Weapon disabled");

        StartCoroutine(SelectNewWeaponAfterDisabled());
    }

    IEnumerator SelectNewWeaponAfterDisabled()
    {
        //When the weapon out of ammo gets destroyed, it does so at the end of a frame
        //So here we do 'yield return 0' which waits till the start of the next frame
        yield return 0;

        selectedWeapon = transform.childCount - 1;

        Debug.Log("Weapon was disabled, selected weapon is now: " + selectedWeapon);

        SelectWeapon(selectedWeapon);
    }

    //Called by the Special Laser Weapon when firing (so player can't switch weapons while doing so)
    public void CanSwitchWeapon(bool _bool)
    {
        canSwitchSpecialWeapon = _bool;
    }
}
