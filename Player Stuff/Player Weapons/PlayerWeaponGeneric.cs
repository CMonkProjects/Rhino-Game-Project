using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All player weapons derive from this
public class PlayerWeaponGeneric : MonoBehaviour
{
    public int ammoIndex;
    //Sound effects
    public string soundWeaponActivation;
    public string soundWeaponFiring;

    //Gunpoint
    public Transform[] gunpoint;

    //Particle Effect Gunpoint
    public ParticleSystem[] particleEffect_Gunpoint;

    //Weapon Barrel Animator
    public WeaponAnimator[] weaponBarrelAnimator;

    protected PlayerWeaponSelector playerWeaponSelector;
    protected PlayerWeaponAmmo playerWeaponAmmo;
    protected WeaponUI weaponUI;

    protected virtual void Awake()
    {
        playerWeaponSelector = transform.parent.GetComponent<PlayerWeaponSelector>();
        playerWeaponAmmo = transform.parent.GetComponent<PlayerWeaponAmmo>();
        weaponUI = GetComponent<WeaponUI>();
    }
}
