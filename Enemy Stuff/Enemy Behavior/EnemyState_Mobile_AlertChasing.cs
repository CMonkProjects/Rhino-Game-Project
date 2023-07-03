using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy relentlessly chases after the player
public class EnemyState_Mobile_AlertChasing : EnemyState
{
    EnemyBehavior_Mobile enemyBehavior_Mobile;
    EnemyRadar enemyRadar;
    public EnemyState_Mobile_AlertChasing(EnemyBehavior_Mobile enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Mobile = enemyBehavior;
        enemyRadar = enemyBehavior.gameObject.GetComponent<EnemyRadar>();
    }

    public override void OnStateEnter()
    {
        StartChasingPlayer();
    }

    public override void Tick()
    {
        if (enemyBehavior_Mobile.currentPlayerTarget != null)
        {
            if (enemyBehavior_Mobile.aiDestinationSetter.target == null)
            {
                StartChasingPlayer();
            }
            
            //Check distance to player to see if we should stop when we're too close
            float distance = Vector2.Distance(transform.position, enemyBehavior_Mobile.currentPlayerTarget.transform.position);

            if (distance < enemyBehavior_Mobile.minDistanceToPlayer && enemyRadar.hasLineOfSight)
            {
                enemyBehavior_Mobile.SwitchToNewState(typeof(EnemyState_Mobile_AlertStop));  //alert stop
            }
        }
    }

    public void StartChasingPlayer()
    {
        if (enemyBehavior_Mobile.currentPlayerTarget == null)
        {
            return;
        }

        if (enemyBehavior_Mobile.aiDestinationSetter != null)
        {
            enemyBehavior_Mobile.aiDestinationSetter.target = enemyBehavior_Mobile.currentPlayerTarget;
        }
    }
}
