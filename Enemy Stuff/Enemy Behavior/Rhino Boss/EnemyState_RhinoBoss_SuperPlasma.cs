using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
//Boss stops to shoot plasma weapon
public class EnemyState_RhinoBoss_SuperPlasma : EnemyState
{
    EnemyBehavior_RhinoBoss enemyBehavior_RhinoBoss;
    EnemyTurret_RhinoBoss enemyTurret;

    int weaponIndex = 3;    //Plasma

    public EnemyState_RhinoBoss_SuperPlasma(EnemyBehavior_RhinoBoss enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_RhinoBoss = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        enemyBehavior_RhinoBoss.aiDestinationSetter.target = null;
        enemyBehavior_RhinoBoss.aiLerp.canMove = false;    //enemy stops immediately
        enemyBehavior_RhinoBoss.aiLerp.SetPath(null);

        enemyTurret = (EnemyTurret_RhinoBoss)enemyBehavior_RhinoBoss.enemyTurret;

        //listen to the delegate
        enemyTurret.doneShooting += DoneShootingWeapon;

        if (enemyTurret)
        {
            enemyTurret.SwitchWeapon(weaponIndex);
            enemyTurret.SetAutoFire(true);
            enemyTurret.StopAllCoroutines();
        }

        enemyBehavior_RhinoBoss.aiDestinationSetter.target = null;
    }

    public override void Tick()
    {

    }

    //check delegate for when shotgun is finished firing
    void DoneShootingWeapon(int _weaponIndex)
    {
        if (_weaponIndex == weaponIndex)
        {
            Debug.LogError("BOSS IS DONE SHOOTING SUPER PLASMA");
        }

        //Exit State
        enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_ChooseNextAttackFinal));
    }

    public override void OnStateExit()
    {
        enemyBehavior_RhinoBoss.aiLerp.canMove = true; //enemy can move again
        enemyBehavior_RhinoBoss.aiLerp.SetPath(null);

        enemyTurret.doneShooting -= DoneShootingWeapon;
    }
}