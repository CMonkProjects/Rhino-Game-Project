using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//This is for enemies locked on and targeted by player missiles
public class EnemyObjectTargeting : MonoBehaviour
{
    [SerializeField] GameObject targetBoxPrefab;

    //When enemy is within the player's missile system range and can be targeted
    public virtual void InMissileRange(bool _inRange)
    {
        if (targetBoxPrefab)
        {
            targetBoxPrefab.SetActive(_inRange);
        }

        if (!_inRange)
        {
            LockedOn(false);
        }
    }

    //When enemy is actively targeted by the player's missile system
    public virtual void Targeted(bool _targeted)
    {
        if (targetBoxPrefab)
        {
            //If player is targeting the enemy and has line of sight then we begin locking on
            targetBoxPrefab.GetComponent<MissileTargetLockOn>().Animation_Targeting(_targeted);
        }

        if (!_targeted)
        {
            LockedOn(false);
        }
    }

    //MAKE A DELAYED COROUTINE THAT CHECKS IF BOTH LOS AND TARGETED ARE TRUE, THEN IT BEGINS THE LOCK ON ANIMATION
    public virtual void HasLineOfSight(bool _lineOfSight)
    {
        if (targetBoxPrefab)
        {
            //If player is targeting the enemy and has line of sight then we begin locking on
            targetBoxPrefab.GetComponent<MissileTargetLockOn>().Animation_LineOfSight(_lineOfSight);
        }

        if (!_lineOfSight)
        {
            LockedOn(false);
        }
    }

    //Check if enemy is locked on or not
    public virtual void LockedOn(bool _isLockedOn)
    {
        PlayerMissileTargeting playerMissileTargeting = GameObject.Find("SpecialWeapon_MissileLauncher(Clone)").GetComponent<PlayerMissileTargeting>();

        if (playerMissileTargeting)
        {
            playerMissileTargeting.EnemyLockedOn(this.gameObject, _isLockedOn);
        }
    }
}
