using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;  //need this for AILerp and AIDestinationSetter
using System;
using UnityEditor;  //OnDrawGizmos

public class EnemyBehavior_Circler : EnemyBehavior
{
    protected internal AIDestinationSetter aiDestinationSetter;
    protected internal AILerp aiLerp;

    EnemyRadar enemyRadar;
    public int priority;

    protected EnemyCollisionDetector enemyCollisionDetector;

    public enum StartingState
    {
        AlertChasing,
        Idle,
        Patrolling
    }

    //Chosen in the inspector
    public StartingState startingState;

    [Header("Waypoint Stuff")]
    [SerializeField] internal Transform waypointGroupParent;    //Group of waypoints

    protected override void Start()
    {
        //Enemy Pathfinding
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiLerp = GetComponent<AILerp>();
        enemyRadar = GetComponent<EnemyRadar>();

        priority = ScoreManager.instanceScoreManager.priority;
        ScoreManager.instanceScoreManager.priority++;

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
            {typeof(EnemyState_Circler_AlertChasing), new EnemyState_Circler_AlertChasing(this) },
            {typeof(EnemyState_Circler_Idle), new EnemyState_Circler_Idle(this) },
            {typeof(EnemyState_Circler_Patrolling), new EnemyState_Circler_Patrolling(this) }
        };

        SetStates(states);
        SelectStartingState(startingState);
    }

    protected virtual void SelectStartingState(StartingState startingState)
    {
        switch (startingState)
        {
            case StartingState.AlertChasing:
                SwitchToNewState(typeof(EnemyState_Circler_AlertChasing));
                enemyRadar.EnemyAlerted();
                break;

            case StartingState.Idle:
                SwitchToNewState(typeof(EnemyState_Circler_Idle));
                break;

            case StartingState.Patrolling:
                SwitchToNewState(typeof(EnemyState_Circler_Patrolling));
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
}
