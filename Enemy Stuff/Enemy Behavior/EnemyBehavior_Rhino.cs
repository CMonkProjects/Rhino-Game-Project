using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;  //need this for AILerp and AIDestinationSetter
using System;
using UnityEditor;  //OnDrawGizmos

public class EnemyBehavior_Rhino : EnemyBehavior
{
    protected internal AIDestinationSetter aiDestinationSetter;
    protected internal AILerp aiLerp;

    protected EnemyRadar enemyRadar;

    protected EnemyCollisionDetector enemyCollisionDetector;

    protected internal bool clockwise = true;

    public enum StartingState
    {
        AlertChasing,
        Idle
    }

    //Chosen in the inspector
    public StartingState startingState;

    //[Header("Waypoint Stuff")]
    //[SerializeField] internal Transform waypointGroupParent;    //Group of waypoints

    protected override void Start()
    {
        //Enemy Rhino
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

    protected virtual void OnEnable()
    {
        EnemySpawnWaveManager.enemyCount++;
    }

    protected virtual void OnDisable()
    {
        EnemySpawnWaveManager.enemyCount--;

        if (EnemySpawnWaveManager.enemyCount < 0)
            EnemySpawnWaveManager.enemyCount = 0;
    }

    protected override void InitializeStates()
    {
        var states = new Dictionary<Type, EnemyState>()
        {
            {typeof(EnemyState_Rhino_AlertChasing), new EnemyState_Rhino_AlertChasing(this) },
            {typeof(EnemyState_Rhino_Idle), new EnemyState_Rhino_Idle(this) },
            {typeof(EnemyState_Rhino_Stopped), new EnemyState_Rhino_Stopped(this) }

        };

        SetStates(states);
        SelectStartingState(startingState);
    }

    protected virtual void SelectStartingState(StartingState startingState)
    {
        switch (startingState)
        {
            case StartingState.AlertChasing:
                SwitchToNewState(typeof(EnemyState_Rhino_AlertChasing));
                enemyRadar.EnemyAlerted();
                break;

            case StartingState.Idle:
                SwitchToNewState(typeof(EnemyState_Rhino_Idle));
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

    protected internal void ClockwiseSwitch()
    {
        clockwise = !clockwise;
    }
}
