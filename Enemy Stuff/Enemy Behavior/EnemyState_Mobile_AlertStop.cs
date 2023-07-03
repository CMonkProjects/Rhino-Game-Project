using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Mobile_AlertStop : EnemyState
{
    EnemyBehavior_Mobile enemyBehavior_Mobile;
    EnemyRadar enemyRadar;

    public EnemyState_Mobile_AlertStop(EnemyBehavior_Mobile enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Mobile = enemyBehavior;
        enemyRadar = enemyBehavior_Mobile.gameObject.GetComponent<EnemyRadar>();
    }

    public override void OnStateEnter()
    {
        enemyBehavior_Mobile.aiLerp.canMove = false;
    }

    public override void Tick()
    {
        if (enemyBehavior_Mobile.currentPlayerTarget == null)
        {
            return;
        }

        //Check if player is greater than maxDistanceToPlayer, if not then we revert back to chasing
        float distance = Vector2.Distance(transform.position, enemyBehavior_Mobile.currentPlayerTarget.transform.position);

        if (distance > enemyBehavior_Mobile.maxDistanceToPlayer || !enemyRadar.hasLineOfSight)
        {
            enemyBehavior_Mobile.SwitchToNewState(typeof(EnemyState_Mobile_AlertChasing));
        }
    }

    public override void OnStateExit()
    {
        enemyBehavior_Mobile.aiLerp.canMove = true;
    }
}
