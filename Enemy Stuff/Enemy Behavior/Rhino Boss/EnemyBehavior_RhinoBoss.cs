using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;  //need this for AILerp and AIDestinationSetter
using System;
using UnityEditor;  //OnDrawGizmos

public class EnemyBehavior_RhinoBoss : EnemyBehavior
{
    protected internal AIDestinationSetter aiDestinationSetter;
    protected internal AILerp aiLerp;

    protected EnemyRadar enemyRadar;

    //public int priority;

    internal int slot = -1;

    protected internal string previousState;

    [SerializeField] EnemyHealth_RhinoBoss enemyHealth_RhinoBoss;

    public enum StartingState
    {
        Entry,
        Blaster,
        Shotgun,
        Missile
    }

    //Chosen in the inspector
    public StartingState startingState;

    [Header("Waypoint Stuff")]
    [SerializeField] internal Transform waypointGroupCenter, waypointGroupEntrance;    //Group of waypoints

    protected override void Start()
    {
        //Enemy Rhino
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiLerp = GetComponent<AILerp>();
        enemyRadar = GetComponent<EnemyRadar>();

        //listen to the delegate
        enemyHealth_RhinoBoss.lowOnHealth += BossLowOnHealth;

        base.Start();
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

    //Boss States
    protected override void InitializeStates()
    {
        var states = new Dictionary<Type, EnemyState>()
        {
            {typeof(EnemyState_RhinoBoss_Entrance), new EnemyState_RhinoBoss_Entrance(this) },
            {typeof(EnemyState_RhinoBoss_ChooseNextAttack), new EnemyState_RhinoBoss_ChooseNextAttack(this) },
            {typeof(EnemyState_RhinoBoss_Blaster), new EnemyState_RhinoBoss_Blaster(this) },
            {typeof(EnemyState_RhinoBoss_Shotgun), new EnemyState_RhinoBoss_Shotgun(this) },
            {typeof(EnemyState_RhinoBoss_Missile), new EnemyState_RhinoBoss_Missile(this) },
            {typeof(EnemyState_RhinoBoss_RapidFire), new EnemyState_RhinoBoss_RapidFire(this) },
            {typeof(EnemyState_RhinoBoss_SuperBlaster), new EnemyState_RhinoBoss_SuperBlaster(this) },
            {typeof(EnemyState_RhinoBoss_SuperPlasma), new EnemyState_RhinoBoss_SuperPlasma(this) },
            {typeof(EnemyState_RhinoBoss_SuperShotgun), new EnemyState_RhinoBoss_SuperShotgun(this) },
            {typeof(EnemyState_RhinoBoss_ChooseNextAttackFinal), new EnemyState_RhinoBoss_ChooseNextAttackFinal(this) },
            {typeof(EnemyState_RhinoBoss_SuperRockets), new EnemyState_RhinoBoss_SuperRockets(this) },
            
        };

        SetStates(states);
        SelectStartingState(startingState);
    }

    //Choose First Boss State
    protected virtual void SelectStartingState(StartingState startingState)
    {
        switch (startingState)
        {
            //For testing purposes be able to pick every basic attack state
            case StartingState.Entry:
                SwitchToNewState(typeof(EnemyState_RhinoBoss_Entrance));
                enemyRadar.EnemyAlerted();
                break;

            case StartingState.Blaster:
                SwitchToNewState(typeof(EnemyState_RhinoBoss_Blaster));
                enemyRadar.EnemyAlerted();
                break;

            case StartingState.Shotgun:
                SwitchToNewState(typeof(EnemyState_RhinoBoss_Shotgun));
                enemyRadar.EnemyAlerted();
                break;

            case StartingState.Missile:
                SwitchToNewState(typeof(EnemyState_RhinoBoss_Missile));
                enemyRadar.EnemyAlerted();
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

    //For our Boss we record the last state we just switched from (so our Boss doesn't pick the same attack twice in a row)
    public override void SwitchToNewState(Type newState)
    {
        if (currentState != null)
            currentState.OnStateExit();

        if (currentState != null)
        {
            GetPreviousState(currentState);
        }

        currentState = availableStates[newState];

        Debug.LogError("Previous state = " + previousState);

        if (currentState != null)
            currentState.OnStateEnter();
    }

    protected void GetPreviousState(EnemyState _currentState)
    {
        previousState = _currentState.ToString();
    }

    //Boss low on health, goes into final attack mode
    protected internal void BossLowOnHealth()
    {
        SwitchToNewState(typeof(EnemyState_RhinoBoss_ChooseNextAttackFinal));
    }

    protected internal IEnumerator EnemyCanMove()
    {
        float waitTime = UnityEngine.Random.Range(0.2f, 0.2f);

        yield return new WaitForSeconds(waitTime);

        aiLerp.isStopped = false;
    }

    //Clear up the slotManager's reserved slot if our boss has taken one
    public override void OnDestroy()
    {
        base.OnDestroy();

        ClearSlot();

        enemyHealth_RhinoBoss.lowOnHealth -= BossLowOnHealth;
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

    protected override IEnumerator OnDyingCoroutine()
    {
        yield return new WaitForEndOfFrame();

        if (aiLerp != null)
        {
            aiLerp.canMove = false;
        }
    }
}
