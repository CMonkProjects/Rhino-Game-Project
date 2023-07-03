using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Mobile_Chasing : EnemyState
{
    EnemyBehavior_Mobile enemyBehavior_Mobile;
    public EnemyState_Mobile_Chasing(EnemyBehavior_Mobile enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Mobile = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        Debug.LogWarning(enemyBehavior_Mobile.name + " - Entering chasing state");
        StartChasingPlayer();
    }

    public override void Tick()
    {
        if (enemyBehavior_Mobile.currentPlayerTarget == null)
        {
            enemyBehavior_Mobile.SwitchToNewState(typeof(EnemyState_Mobile_Idle));
        }
        else
        {
            //Check distance to player to see if we should stop when we're too close
            float distance = Vector2.Distance(transform.position, enemyBehavior_Mobile.currentPlayerTarget.transform.position);

            if (distance < enemyBehavior_Mobile.minDistanceToPlayer && enemyBehavior_Mobile.enemyTurret.hasLineOfSight)
            {
                enemyBehavior_Mobile.SwitchToNewState(typeof(EnemyState_Mobile_Stop));
            }
        }
    }

    public void StartChasingPlayer()
    {
        if (enemyBehavior_Mobile.currentPlayerTarget != null)
        {
            if (enemyBehavior_Mobile.aiDestinationSetter != null)
            {
                enemyBehavior_Mobile.aiDestinationSetter.target = enemyBehavior_Mobile.currentPlayerTarget;
            }
        }
    }
}
