using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Unarmed_Waypoints : EnemyState
{
    EnemyBehavior_Unarmed enemyBehavior_Unarmed;

    public EnemyState_Unarmed_Waypoints(EnemyBehavior_Unarmed enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Unarmed = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        SetFirstWaypoint();
    }

    public override void Tick()
    {
        if (enemyBehavior_Unarmed.waypointTargetCurrent != null)
        {
            if (enemyBehavior_Unarmed.aiLerp.reachedEndOfPath)
            {
                SetNextWaypointTarget();
            }
        }
    }

    //Set enemy to head towards the first waypoint in the list
    public void SetFirstWaypoint()
    {
        enemyBehavior_Unarmed.waypointTargetCurrent = null;

        if (enemyBehavior_Unarmed.aiDestinationSetter != null)
        {
            enemyBehavior_Unarmed.waypointTargetCurrent = enemyBehavior_Unarmed.waypointList[enemyBehavior_Unarmed.waypointIndex];  //index should always be at 0

            enemyBehavior_Unarmed.aiDestinationSetter.target = enemyBehavior_Unarmed.waypointTargetCurrent;
        }
    }

    //Set the next waypoint, the waypoints don't loop and they dissapear when they reach the end
    public void SetNextWaypointTarget()
    {
        enemyBehavior_Unarmed.waypointTargetCurrent = null;

        if (enemyBehavior_Unarmed.aiDestinationSetter != null)
        {
            if (enemyBehavior_Unarmed.waypointIndex < enemyBehavior_Unarmed.waypointList.Count - 1)
            {
                enemyBehavior_Unarmed.waypointIndex++;
            }
            else
            {
                enemyBehavior_Unarmed.DisappearForever();   //enemy gets removed when reaching the final waypoint
                return;
            }

            enemyBehavior_Unarmed.waypointTargetCurrent = enemyBehavior_Unarmed.waypointList[enemyBehavior_Unarmed.waypointIndex];

            enemyBehavior_Unarmed.aiDestinationSetter.target = enemyBehavior_Unarmed.waypointTargetCurrent;
        }
    }
}
