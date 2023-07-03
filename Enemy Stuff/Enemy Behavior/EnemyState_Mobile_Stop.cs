using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Mobile_Stop : EnemyState
{
    EnemyBehavior_Mobile enemyBehavior_Mobile;

    public EnemyState_Mobile_Stop(EnemyBehavior_Mobile enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Mobile = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        enemyBehavior_Mobile.aiLerp.canMove = false;
    }

    public override void Tick()
    {
        if (enemyBehavior_Mobile.currentPlayerTarget == null)
        {
            enemyBehavior_Mobile.SwitchToNewState(typeof(EnemyState_Mobile_Idle));
        }

        //Check if player is within maxDistanceToPlayer, if not then we revert back to chasing
        float distance = Vector2.Distance(transform.position, enemyBehavior_Mobile.currentPlayerTarget.transform.position);

        if (distance > enemyBehavior_Mobile.minDistanceToPlayer || !enemyBehavior_Mobile.enemyTurret.hasLineOfSight)
        {
            enemyBehavior_Mobile.SwitchToNewState(typeof(EnemyState_Mobile_Chasing));
        }
    }

    public override void OnStateExit()
    {
        enemyBehavior_Mobile.aiLerp.canMove = true;
    }
}