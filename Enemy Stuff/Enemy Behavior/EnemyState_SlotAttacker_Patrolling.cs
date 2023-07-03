using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_SlotAttacker_Patrolling : EnemyState
{
    EnemyBehavior_SlotAttacker enemyBehavior_SlotAttacker;

    Transform waypointTargetCurrent;  //current waypoint enemy is moving to
    List<Transform> waypointList = new List<Transform>();
    int waypointIndex = 0;        //how we cycle through the waypoints in the list

    public EnemyState_SlotAttacker_Patrolling(EnemyBehavior_SlotAttacker enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_SlotAttacker = enemyBehavior;
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
            if (enemyBehavior_SlotAttacker.aiLerp.reachedEndOfPath)
            {
                SetNextWaypointTarget();
            }
        }

        //Chase player if spotted
        if (enemyBehavior_SlotAttacker.currentPlayerTarget != null)
        {
            enemyBehavior_SlotAttacker.SwitchToNewState(typeof(EnemyState_SlotAttacker_Chasing));
        }
    }

    //OnStateExit()

    //Waypoint Stuff
    void GetWaypointList()
    {
        if (enemyBehavior_SlotAttacker.waypointGroupParent == null)
        {
            return;
        }

        foreach (Transform childWaypoint in enemyBehavior_SlotAttacker.waypointGroupParent)
        {
            waypointList.Add(childWaypoint);
        }
    }

    void SetFirstWaypoint()
    {
        waypointTargetCurrent = null;

        if (enemyBehavior_SlotAttacker.aiDestinationSetter != null)
        {
            waypointTargetCurrent = waypointList[waypointIndex];

            enemyBehavior_SlotAttacker.aiDestinationSetter.target = waypointTargetCurrent;
        }
    }

    void SetNextWaypointTarget()
    {
        //pick the waypoint child index in the waypoint parent
        waypointTargetCurrent = null;

        if (enemyBehavior_SlotAttacker.aiDestinationSetter != null)
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

            enemyBehavior_SlotAttacker.aiDestinationSetter.target = waypointTargetCurrent;
        }
    }
}
