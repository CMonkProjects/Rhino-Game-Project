using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    Animator weaponAnimator;
    [SerializeField] string animBool_isShooting = "isShooting";
    [SerializeField] string animBool_isIdle = "isIdle";
    [SerializeField] float desiredAnimationSpeed;
    [SerializeField] float overchargedAnimationSpeed;

    void Start()
    {
        weaponAnimator = GetComponent<Animator>();
        weaponAnimator.speed = desiredAnimationSpeed;
    }

    //This is called at the end of the weapon sprite's shooting animation
    internal void AnimationEnd()
    {
        if (weaponAnimator)
        {
            weaponAnimator.SetBool(animBool_isIdle, true);
            weaponAnimator.SetBool(animBool_isShooting, false);
        }
    }

    internal void WeaponCharging()
    {

    }

    internal void Animation_GoIdle()
    {
        if (weaponAnimator)
        {
            weaponAnimator.SetBool(animBool_isShooting, false);
        }
    }

    internal void Animation_Shooting()
    {
        if (weaponAnimator)
        {
            weaponAnimator.SetBool(animBool_isShooting, true);
        }
    }

    //When the player's main weapon is overcharged and firerate has increased
    internal void Animation_Overcharged(bool _overcharged)
    {
        switch (_overcharged)
        {
            case true:
                weaponAnimator.speed = overchargedAnimationSpeed;
                break;

            case false:
                weaponAnimator.speed = desiredAnimationSpeed;
                break;
        }
    }
}
