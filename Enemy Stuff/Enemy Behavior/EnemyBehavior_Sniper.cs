using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;  //need this for AILerp and AIDestinationSetter
using System;

public class EnemyBehavior_Sniper : EnemyBehavior
{
    protected internal AIDestinationSetter aiDestinationSetter;
    protected internal AILerp aiLerp;

    protected EnemyCollisionDetector enemyCollisionDetector;

    [Header("Enemy moves away from Player if within this range")]
    [SerializeField] internal float minDistanceToPlayer = 1f;

    [Header("Enemy stops when player is within this range")]
    [SerializeField] internal float stoppingDistanceToPlayer = 1.5f;

    [Header("Enemy chases player if beyond this range")]
    [SerializeField] internal float maxDistanceToPlayer = 2f;

    [Header("Slots that sniper moves towards when trying to get away from player")]
    public Transform slotsKeepDistanceParent;
    public List<Transform> slotsKeepDistance;

    //Stops when a fellow enemy is in the way
    //[SerializeField] LayerMask allyMask;
    EnemyRadar enemyRadar;

    public enum StartingState
    {
        Idle,
        AlertChasing,
        Patrolling
    }

    public StartingState startingState;

    [Header("Waypoint Stuff")]
    [SerializeField] internal Transform waypointGroupParent;    //Group of waypoints

    protected override void Start()
    {
        //Sniper
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiLerp = GetComponent<AILerp>();
        enemyRadar = GetComponent<EnemyRadar>();

        base.Start();

        EnemyCollisionDetector enemyCollisionDetector = GetComponent<EnemyCollisionDetector>();

        if (enemyCollisionDetector != null)
        {
            enemyCollisionDetector.onEnemyCollision += EnemyCollision;
            enemyCollisionDetector.onCanMove += EnemyCanMove;
        }
    }

    protected override void InitializeStates()
    {
        var states = new Dictionary<Type, EnemyState>()
        {
            {typeof(EnemyState_Sniper_Idle), new EnemyState_Sniper_Idle(this) },
            {typeof(EnemyState_Sniper_AlertChasing), new EnemyState_Sniper_AlertChasing(this) },
            {typeof(EnemyState_Sniper_KeepDistance), new EnemyState_Sniper_KeepDistance(this) },
            {typeof(EnemyState_Sniper_Stop), new EnemyState_Sniper_Stop(this) },
            {typeof(EnemyState_Sniper_Patrolling), new EnemyState_Sniper_Patrolling(this) }
        };

        SetStates(states);
        SelectStartingState(startingState);
    }

    protected virtual void SelectStartingState(StartingState startingState)
    {
        switch (startingState)
        {
            case StartingState.Idle:
                SwitchToNewState(typeof(EnemyState_Sniper_Idle));
                break;

            case StartingState.AlertChasing:
                SwitchToNewState(typeof(EnemyState_Sniper_AlertChasing));
                enemyRadar.EnemyAlerted();
                break;

            case StartingState.Patrolling:
                SwitchToNewState(typeof(EnemyState_Sniper_Patrolling));
                break;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (slotsKeepDistanceParent)
        {
            Destroy(slotsKeepDistanceParent.gameObject);
        }

        if (enemyCollisionDetector != null)
        {
            enemyCollisionDetector.onEnemyCollision -= EnemyCollision;
            enemyCollisionDetector.onCanMove -= EnemyCanMove;
        }
    }

    protected internal void EnemyCollision()
    {
        if (aiLerp != null)
        {
            aiLerp.isStopped = true;
        }
    }

    protected internal IEnumerator EnemyCanMoveCoroutine()
    {
        float waitTime = UnityEngine.Random.Range(0.5f, 1f);

        yield return new WaitForSeconds(waitTime);

        if (aiLerp != null)
        {
            aiLerp.isStopped = false;
        }
    }

    protected internal void EnemyCanMove()
    {
        StartCoroutine(EnemyCanMoveCoroutine());
    }
}
