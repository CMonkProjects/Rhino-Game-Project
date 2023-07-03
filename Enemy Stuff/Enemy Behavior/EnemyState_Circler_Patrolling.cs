using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Circler_Patrolling : EnemyState
{
    EnemyBehavior_Circler enemyBehavior_Circler;

    Transform waypointTargetCurrent;  //current waypoint enemy is moving to
    List<Transform> waypointList = new List<Transform>();
    int waypointIndex = 0;        //how we cycle through the waypoints in the list

    public EnemyState_Circler_Patrolling(EnemyBehavior_Circler enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Circler = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        GetWaypointList();

        if (waypointList.Count > 0)
        {
            SetFirstWaypoint();
        }
    }

    public override void Tick()
    {
        //patrol around waypoints
        if (waypointTargetCurrent != null)
        {
            if (enemyBehavior_Circler.aiLerp.reachedEndOfPath)
            {
                SetNextWaypointTarget();
            }
        }

        //Chase player if spotted
        if (enemyBehavior_Circler.currentPlayerTarget != null)
        {
            enemyBehavior_Circler.SwitchToNewState(typeof(EnemyState_Circler_AlertChasing));
        }
    }

    //OnStateExit()

    //Waypoint Stuff
    void GetWaypointList()
    {
        if (enemyBehavior_Circler.waypointGroupParent == null)
        {
            return;
        }

        foreach (Transform childWaypoint in enemyBehavior_Circler.waypointGroupParent)
        {
            waypointList.Add(childWaypoint);
        }
    }

    void SetFirstWaypoint()
    {
        waypointTargetCurrent = null;

        if (enemyBehavior_Circler.aiDestinationSetter != null)
        {
            waypointTargetCurrent = waypointList[waypointIndex];

            enemyBehavior_Circler.aiDestinationSetter.target = waypointTargetCurrent;
        }
    }

    void SetNextWaypointTarget()
    {
        //pick the waypoint child index in the waypoint parent
        waypointTargetCurrent = null;

        if (enemyBehavior_Circler.aiDestinationSetter != null)
        {
            if (waypointIndex < waypointList.Count - 1)
            {
                waypointIndex++;
            }
            else
            {
                //if waypoint index > child index length then cycle back to 0
                waypointIndex = 0;
            }

            waypointTargetCurrent = waypointList[waypointIndex];

            enemyBehavior_Circler.aiDestinationSetter.target = waypointTargetCurrent;
        }
    }
}
