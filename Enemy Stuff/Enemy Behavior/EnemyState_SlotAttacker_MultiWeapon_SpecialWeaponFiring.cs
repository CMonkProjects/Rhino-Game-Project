using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyState_SlotAttacker_MultiWeapon_SpecialWeaponFiring : EnemyState
{
    EnemyBehavior_SlotAttacker_MultiWeapon enemyBehavior_SlotAttacker_MultiWeapon;
    EnemyTurret_MultiWeapon enemyTurret;
    public EnemyState_SlotAttacker_MultiWeapon_SpecialWeaponFiring(EnemyBehavior_SlotAttacker_MultiWeapon enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_SlotAttacker_MultiWeapon = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        enemyBehavior_SlotAttacker_MultiWeapon.aiDestinationSetter.target = null;
        enemyBehavior_SlotAttacker_MultiWeapon.aiLerp.canMove = false;    //enemy stops immediately
        enemyBehavior_SlotAttacker_MultiWeapon.aiLerp.SetPath(null);

        enemyTurret = (EnemyTurret_MultiWeapon)enemyBehavior_SlotAttacker_MultiWeapon.enemyTurret;

        //Delegate reference
        enemyTurret.doneShooting += FinishedShootingSecondary;

        if (enemyTurret)
        {
            enemyTurret.SwitchWeapon(1);
            enemyTurret.SetAutoFire(false);
            enemyTurret.StopAllCoroutines();

            //start shooting secondary weapon
            enemyTurret.StartCoroutine(enemyTurret.StartShooting());
        }

        //Clear the chosen attack slot
        enemyBehavior_SlotAttacker_MultiWeapon.ClearSlot();
    }

    public override void Tick()
    {

    }

    void FinishedShootingSecondary(int _weaponIndex)
    {
        if (_weaponIndex == 1)
        {
            enemyBehavior_SlotAttacker_MultiWeapon.SwitchToNewState(typeof(EnemyState_SlotAttacker_MultiWeapon_AlertChasing));
        }
    }

    public override void OnStateExit()
    {
        //switch back to primary weapon and start shooting normally again
        if (enemyTurret)
        {
            enemyTurret.SwitchWeapon(0);
            enemyTurret.SetAutoFire(true);
        }

        enemyBehavior_SlotAttacker_MultiWeapon.aiLerp.canMove = true; //enemy can move again
        enemyBehavior_SlotAttacker_MultiWeapon.aiLerp.SetPath(null);

        enemyTurret.doneShooting -= FinishedShootingSecondary;
    }
}
