using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponAnimator : MonoBehaviour
{
    Animator weaponAnimator;
    [SerializeField] string animBool_isShooting = "isShootingPrimary";
    [SerializeField] string animBool_isShootingSecondary = "isShootingSecondary";
    [SerializeField] string animBool_isIdle = "isIdle";
    [SerializeField] float desiredAnimationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        weaponAnimator = GetComponent<Animator>();
        weaponAnimator.speed = desiredAnimationSpeed;
    }

    //When some enemy weapons begin to fire, they have a 'tell' that plays
    internal void Animation_Shooting(string _primaryOrSecondary)
    {
        if (weaponAnimator)
        {
            switch (_primaryOrSecondary)
            {
                case "isShootingPrimary":
                    weaponAnimator.SetBool(animBool_isShooting, true);
                    break;

                case "isShootingSecondary":
                    weaponAnimator.SetBool(animBool_isShootingSecondary, true);
                    break;
            }
            
        }
    }

    internal void Animation_GoIdle(string _primaryOrSecondary)
    {
        if (weaponAnimator)
        {
            switch (_primaryOrSecondary)
            {
                case "isShootingPrimary":
                    weaponAnimator.SetBool(animBool_isShooting, false);
                    break;

                case "isShootingSecondary":
                    weaponAnimator.SetBool(animBool_isShootingSecondary, false);
                    break;
            }

            weaponAnimator.SetBool(animBool_isIdle, true);
        }
    }
}
