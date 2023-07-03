using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;  //need this for AILerp and AIDestinationSetter
using System;
using UnityEditor;  //OnDrawGizmos

public class EnemyBehavior_Mobile : EnemyBehavior
{
    protected internal AIDestinationSetter aiDestinationSetter;
    protected internal AILerp aiLerp;

    protected EnemyCollisionDetector enemyCollisionDetector;

    [Header("Enemy stops if player is within this range")]
    [SerializeField] internal float minDistanceToPlayer = 0.75f;

    [Header("Enemy chases if player is beyond this range")]
    [SerializeField] internal float maxDistanceToPlayer = 2f;

    //Stops when a fellow enemy is in the way
    //[SerializeField] LayerMask allyMask;
    EnemyRadar enemyRadar;

    public enum StartingState
    {
        Idle,
        AlertChasing,
        Patrolling
    }

    //Chosen in the inspector
    public StartingState startingState;

    [Header("Waypoint Stuff")]
    [SerializeField] internal Transform waypointGroupParent;    //Group of waypoints

    protected override void Start()
    {
        //Mobile
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

    void OnEnable()
    {
        EnemySpawnWaveManager.enemyCount++;
    }

    void OnDisable()
    {
        EnemySpawnWaveManager.enemyCount--;

        if (EnemySpawnWaveManager.enemyCount < 0)
            EnemySpawnWaveManager.enemyCount = 0;
    }

    protected override void InitializeStates()
    {
        var states = new Dictionary<Type, EnemyState>()
        {
            {typeof(EnemyState_Mobile_Idle), new EnemyState_Mobile_Idle(this) },
            {typeof(EnemyState_Mobile_AlertChasing), new EnemyState_Mobile_AlertChasing(this) },
            {typeof(EnemyState_Mobile_AlertStop), new EnemyState_Mobile_AlertStop(this) },
            {typeof(EnemyState_Mobile_Patrolling), new EnemyState_Mobile_Patrolling(this) }
        };

        SetStates(states);
        SelectStartingState(startingState);
    }

    protected virtual void SelectStartingState(StartingState startingState)
    {
        switch (startingState)
        {
            case StartingState.Idle:
                SwitchToNewState(typeof(EnemyState_Mobile_Idle));
                break;

            case StartingState.AlertChasing:
                SwitchToNewState(typeof(EnemyState_Mobile_AlertChasing));
                enemyRadar.EnemyAlerted();
                break;

            case StartingState.Patrolling:
                SwitchToNewState(typeof(EnemyState_Mobile_Patrolling));
                break;
        }
    }

    protected override void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            tickTimer += Time.deltaTime;

            if (tickTimer >= tickRate)
            {
                currentState.Tick();
                //Check for other enemies in the way
                //CheckForEnemiesInWay();
                tickTimer = 0f;
            }
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

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

    /*protected void CheckForEnemiesInWay()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position, transform.up, 0.5f, allyMask);

        if (hit)
        {
            EnemyCollision(hit.collider);
            return;
        }

        if (aiLerp.isStopped)
        {
            StartCoroutine(EnemyCanMove());
        }
    }*/
}
