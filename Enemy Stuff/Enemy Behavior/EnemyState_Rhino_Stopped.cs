using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyState_Rhino_Stopped : EnemyState
{
    EnemyBehavior_Rhino enemyBehavior_Rhino;
    EnemyTurret_MultiWeapon enemyTurret;

    float tickerTimerReset = 1f;
    float tickerTimer;

    public EnemyState_Rhino_Stopped(EnemyBehavior_Rhino enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Rhino = enemyBehavior;
    }

    //Stop the Enemy Rhino and switch to its secondary weapon
    public override void OnStateEnter()
    {
        tickerTimer = tickerTimerReset;

        enemyBehavior_Rhino.aiDestinationSetter.target = null;
        enemyBehavior_Rhino.aiLerp.canMove = false;    //enemy stops immediately
        enemyBehavior_Rhino.aiLerp.SetPath(null);

        enemyTurret = (EnemyTurret_MultiWeapon)enemyBehavior_Rhino.enemyTurret;

        if (enemyTurret)
        {
            enemyTurret.SwitchWeapon(1);
            enemyTurret.SetAutoFire(false);
            enemyTurret.StopAllCoroutines();

            //start shooting secondary weapon
            enemyTurret.StartCoroutine(enemyTurret.StartShooting());
        }

        enemyTurret.doneShooting += FinishedShootingSecondary;
    }

    public override void Tick()
    {
        if (enemyBehavior_Rhino.currentPlayerTarget != null)
        {
            if (tickerTimer > 0)
            {
                tickerTimer -= Time.deltaTime;
                return;
            }
        }
    }

    //called by the done shooting delegate in the MultiWeapon_Turret script
    void FinishedShootingSecondary(int _weaponIndex)
    {
        if (_weaponIndex == 1)
        {
            enemyBehavior_Rhino.SwitchToNewState(typeof(EnemyState_Rhino_AlertChasing));
        }
    }

    //Switch back to primary weapon and move again
    public override void OnStateExit()
    {
        if (enemyTurret)
        {
            enemyTurret.SwitchWeapon(0);
            enemyTurret.SetAutoFire(true);
        }

        enemyTurret.doneShooting -= FinishedShootingSecondary;

        enemyBehavior_Rhino.aiLerp.canMove = true; //enemy can move again
        enemyBehavior_Rhino.aiLerp.SetPath(null);
    }
}
