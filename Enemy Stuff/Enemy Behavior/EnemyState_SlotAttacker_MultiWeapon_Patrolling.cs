using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_SlotAttacker_MultiWeapon_Patrolling : EnemyState
{
    EnemyBehavior_SlotAttacker_MultiWeapon EnemyBehavior_SlotAttacker_MultiWeapon;

    Transform waypointTargetCurrent;  //current waypoint enemy is moving to
    List<Transform> waypointList = new List<Transform>();
    int waypointIndex = 0;        //how we cycle through the waypoints in the list

    public EnemyState_SlotAttacker_MultiWeapon_Patrolling(EnemyBehavior_SlotAttacker_MultiWeapon enemyBehavior) : base(enemyBehavior.gameObject)
    {
        EnemyBehavior_SlotAttacker_MultiWeapon = enemyBehavior;
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
            if (EnemyBehavior_SlotAttacker_MultiWeapon.aiLerp.reachedEndOfPath)
            {
                SetNextWaypointTarget();
            }
        }

        //Chase player if spotted
        if (EnemyBehavior_SlotAttacker_MultiWeapon.currentPlayerTarget != null)
        {
            EnemyBehavior_SlotAttacker_MultiWeapon.SwitchToNewState(typeof(EnemyState_SlotAttacker_MultiWeapon_AlertChasing));
        }
    }

    //OnStateExit()

    //Waypoint Stuff
    void GetWaypointList()
    {
        if (EnemyBehavior_SlotAttacker_MultiWeapon.waypointGroupParent == null)
        {
            return;
        }

        foreach (Transform childWaypoint in EnemyBehavior_SlotAttacker_MultiWeapon.waypointGroupParent)
        {
            waypointList.Add(childWaypoint);
        }
    }

    void SetFirstWaypoint()
    {
        waypointTargetCurrent = null;

        if (EnemyBehavior_SlotAttacker_MultiWeapon.aiDestinationSetter != null)
        {
            waypointTargetCurrent = waypointList[waypointIndex];

            EnemyBehavior_SlotAttacker_MultiWeapon.aiDestinationSetter.target = waypointTargetCurrent;
        }
    }

    void SetNextWaypointTarget()
    {
        //pick the waypoint child index in the waypoint parent
        waypointTargetCurrent = null;

        if (EnemyBehavior_SlotAttacker_MultiWeapon.aiDestinationSetter != null)
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

            EnemyBehavior_SlotAttacker_MultiWeapon.aiDestinationSetter.target = waypointTargetCurrent;
        }
    }
}
