using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;  //need this for AILerp and AIDestinationSetter
using System;
using UnityEditor;  //OnDrawGizmos

public class EnemyBehavior_Unarmed : EnemyBehavior
{
    protected internal AIDestinationSetter aiDestinationSetter;
    protected internal AILerp aiLerp;

    //Stops when a fellow enemy is in the way
    [SerializeField] LayerMask allyMask;
    EnemyRadar enemyRadar;
    public int priority;

    public enum StartingState
    {
        Waypoints,
        Idle
    }

    //Chosen in the inspector
    public StartingState startingState;

    [Header("Waypoint Stuff")]
    [SerializeField] internal Transform waypointTargetCurrent;  //current waypoint enemy is moving to
    [SerializeField] internal Transform waypointGroupParent;    //Group of waypoints
    [SerializeField] internal List<Transform> waypointList = new List<Transform>();
    [SerializeField] internal int waypointIndex = 0;        //how we cycle through the waypoints in the list

    protected override void Start()
    {
        //Unarmed
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiLerp = GetComponent<AILerp>();
        enemyRadar = GetComponent<EnemyRadar>();

        GetWaypointList();

        enemyHealth = transform.GetChild(0).GetComponent<EnemyHealth>();
        InitializeStates();
    }

    protected override void InitializeStates()
    {
        var states = new Dictionary<Type, EnemyState>()
        {
            {typeof(EnemyState_Unarmed_Waypoints), new EnemyState_Unarmed_Waypoints(this) },
            {typeof(EnemyState_Unarmed_Idle), new EnemyState_Unarmed_Idle(this) }
        };

        SetStates(states);
        SelectStartingState(startingState);
    }

    protected virtual void SelectStartingState(StartingState startingState)
    {
        switch (startingState)
        {
            case StartingState.Waypoints:
                SwitchToNewState(typeof(EnemyState_Unarmed_Waypoints));
                break;

            case StartingState.Idle:
                SwitchToNewState(typeof(EnemyState_Unarmed_Idle));
                break;
        }
    }

    protected virtual void GetWaypointList()
    {
        if (waypointGroupParent == null)
        {
            return;
        }

        foreach (Transform childWaypoint in waypointGroupParent)
        {
            waypointList.Add(childWaypoint);
        }
    }

    //When the truck reaches the final waypoint it gets removed
    protected internal virtual void DisappearForever()
    {
        //GetComponent<PowerupDropper>().dropPowerup = false;

        Destroy(gameObject);
    }
}
