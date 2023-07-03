using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Sniper_Patrolling : EnemyState
{
    EnemyBehavior_Sniper enemyBehavior_Sniper;

    Transform waypointTargetCurrent;  //current waypoint enemy is moving to
    List<Transform> waypointList = new List<Transform>();
    int waypointIndex = 0;        //how we cycle through the waypoints in the list

    public EnemyState_Sniper_Patrolling(EnemyBehavior_Sniper enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Sniper = enemyBehavior;
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
            if (enemyBehavior_Sniper.aiLerp.reachedEndOfPath)
            {
                SetNextWaypointTarget();
            }
        }

        //Chase player if spotted
        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            enemyBehavior_Sniper.SwitchToNewState(typeof(EnemyState_Sniper_AlertChasing));
        }
    }

    //OnStateExit()

    //Waypoint Stuff
    void GetWaypointList()
    {
        if (enemyBehavior_Sniper.waypointGroupParent == null)
        {
            return;
        }

        foreach (Transform childWaypoint in enemyBehavior_Sniper.waypointGroupParent)
        {
            waypointList.Add(childWaypoint);
        }
    }

    void SetFirstWaypoint()
    {
        waypointTargetCurrent = null;

        if (enemyBehavior_Sniper.aiDestinationSetter != null)
        {
            waypointTargetCurrent = waypointList[waypointIndex];

            enemyBehavior_Sniper.aiDestinationSetter.target = waypointTargetCurrent;
        }
    }

    void SetNextWaypointTarget()
    {
        //pick the waypoint child index in the waypoint parent
        waypointTargetCurrent = null;

        if (enemyBehavior_Sniper.aiDestinationSetter != null)
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

            enemyBehavior_Sniper.aiDestinationSetter.target = waypointTargetCurrent;
        }
    }
}
