using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;  //need this for AILerp and AIDestinationSetter
using System;

public class EnemyBehavior_SlotAttacker : EnemyBehavior
{
    protected internal AIDestinationSetter aiDestinationSetter;
    protected internal AILerp aiLerp;
    protected EnemyCollisionDetector enemyCollisionDetector;


    //Stops when a fellow enemy is in the way
    //[SerializeField] protected LayerMask allyMask;
    protected EnemyRadar enemyRadar;

    internal int slot = -1;

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
            {typeof(EnemyState_SlotAttacker_Idle), new EnemyState_SlotAttacker_Idle(this) },
            {typeof(EnemyState_SlotAttacker_Chasing), new EnemyState_SlotAttacker_Chasing(this) },
            {typeof(EnemyState_SlotAttacker_Patrolling), new EnemyState_SlotAttacker_Patrolling(this) }
        };

        SetStates(states);
        SelectStartingState(startingState);
    }

    public virtual void SelectStartingState(StartingState nextState)
    {
        switch (nextState)
        {
            case StartingState.AlertChasing:
                SwitchToNewState(typeof(EnemyState_SlotAttacker_Chasing));
                enemyRadar.EnemyAlerted();
                break;

            case StartingState.Idle:
                SwitchToNewState(typeof(EnemyState_SlotAttacker_Idle));
                break;

            case StartingState.Patrolling:
                SwitchToNewState(typeof(EnemyState_SlotAttacker_Patrolling));
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

    //Clear up the slotManager's reserved slot if our enemy has taken one
    public override void OnDestroy()
    {
        ClearSlot();

        base.OnDestroy();

        if (enemyCollisionDetector != null)
        {
            enemyCollisionDetector.onEnemyCollision -= EnemyCollision;
            enemyCollisionDetector.onCanMove -= EnemyCanMove;
        }
    }

    public virtual void ClearSlot()
    {
        var slotManager = currentPlayerTarget.GetComponent<SlotManager>();

        if (slotManager != null)
        {
            //reset our enemy's chosen slot
            slotManager.ReleaseSlot(slot);
            slot = -1;
        }
    }
}
