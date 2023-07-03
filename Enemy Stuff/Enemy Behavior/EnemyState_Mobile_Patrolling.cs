using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Mobile_Patrolling : EnemyState
{
    EnemyBehavior_Mobile enemyBehavior_Mobile;

    Transform waypointTargetCurrent;  //current waypoint enemy is moving to
    List<Transform> waypointList = new List<Transform>();
    int waypointIndex = 0;        //how we cycle through the waypoints in the list

    public EnemyState_Mobile_Patrolling(EnemyBehavior_Mobile enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Mobile = enemyBehavior;
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
            if (enemyBehavior_Mobile.aiLerp.reachedEndOfPath)
            {
                SetNextWaypointTarget();
            }
        }

        //Chase player if spotted
        if (enemyBehavior_Mobile.currentPlayerTarget != null)
        {
            enemyBehavior_Mobile.SwitchToNewState(typeof(EnemyState_Mobile_AlertChasing));
        }
    }

    //OnStateExit()

    //Waypoint Stuff
    void GetWaypointList()
    {
        if (enemyBehavior_Mobile.waypointGroupParent == null)
        {
            return;
        }

        foreach (Transform childWaypoint in enemyBehavior_Mobile.waypointGroupParent)
        {
            waypointList.Add(childWaypoint);
        }
    }

    void SetFirstWaypoint()
    {
        waypointTargetCurrent = null;

        if (enemyBehavior_Mobile.aiDestinationSetter != null)
        {
            waypointTargetCurrent = waypointList[waypointIndex];

            enemyBehavior_Mobile.aiDestinationSetter.target = waypointTargetCurrent;
        }
    }

    void SetNextWaypointTarget()
    {
        //pick the waypoint child index in the waypoint parent
        waypointTargetCurrent = null;

        if (enemyBehavior_Mobile.aiDestinationSetter != null)
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

            enemyBehavior_Mobile.aiDestinationSetter.target = waypointTargetCurrent;
        }
    }
}
