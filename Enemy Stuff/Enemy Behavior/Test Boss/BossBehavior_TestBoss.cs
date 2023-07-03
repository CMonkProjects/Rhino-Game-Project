using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;  //need this for AILerp and AIDestinationSetter
using System;
using UnityEngine.Events;

public class BossBehavior_TestBoss : EnemyBehavior
{
    protected internal AIDestinationSetter aiDestinationSetter;
    protected internal AILerp aiLerp;

    public Transform currentTurretTarget;
    public Transform currentWaypointTarget;
    public Transform missileWaypointGroupParent;

    protected internal BossTurret enemyBossTurret;

    public UnityEvent onBossActive;
    public UnityEvent onBossInCombat;
    public UnityEvent onBossDefeated;

    public enum StartingState
    {
        BossEntrance
    }

    [SerializeField] internal Transform waypointEntrance;
    [SerializeField] internal List<Transform> currentPathList;

    //Choose this in the inspector
    public StartingState startingState;

    List<string> combatStates = new List<string> { "EnemyState_Boss_BlasterChasing", "EnemyState_Boss_Shotgun", "EnemyState_Boss_Missile" };

    int combatCycle = 0;

    protected override void Start()
    {
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiLerp = GetComponent<AILerp>();

        GetTurretStuff();
        enemyHealth = transform.GetChild(0).GetComponent<BossHealth>();

        InitializeStates();
    }

    void OnEnable()
    {
        BossActive();
    }

    protected override void GetTurretStuff()
    {
        enemyTurretObject = transform.Find("Enemy_Turret").gameObject;

        if (enemyTurretObject != null)
        {
            enemyBossTurret = enemyTurretObject.GetComponent<BossTurret>();    //Change this to grabbing the boss turret
            enemyTurretObject.transform.localRotation = Quaternion.identity;
        }
    }
    protected override void InitializeStates()
    {
        var states = new Dictionary<Type, EnemyState>()
        {
            {typeof(EnemyState_Boss_Entrance), new EnemyState_Boss_Entrance(this) },
            {typeof(EnemyState_Boss_BlasterChasing), new EnemyState_Boss_BlasterChasing(this) },
            {typeof(EnemyState_Boss_Shotgun), new EnemyState_Boss_Shotgun(this) },
            {typeof(EnemyState_Boss_Missile), new EnemyState_Boss_Missile(this) }
        };

        SetStates(states);
        SelectStartingState(startingState);
    }

    protected virtual void SelectStartingState(StartingState startingState)
    {
        switch (startingState)
        {
            case StartingState.BossEntrance:
                SwitchToNewState(typeof(EnemyState_Boss_Entrance));
                break;
        }
    }

    protected virtual void SelectCombatState(string _newState)
    {
        switch (_newState)
        {
            case "EnemyState_Boss_BlasterChasing":
                SwitchToNewCombatState(typeof(EnemyState_Boss_BlasterChasing));
                break;

            case "EnemyState_Boss_Shotgun":
                SwitchToNewCombatState(typeof(EnemyState_Boss_Shotgun));
                break;

            case "EnemyState_Boss_Missile":
                SwitchToNewCombatState(typeof(EnemyState_Boss_Missile));
                break;
        }
    }

    public void SwitchToNewCombatState(Type combatState)
    {
        var nextState = availableStates[combatState];

        if (currentState != null)
            currentState.OnStateExit();

        currentState = nextState;

        Debug.LogError("NEW STATE IS: " + currentState);

        if (currentState != null)
            currentState.OnStateEnter();
    }

    //Choose next attack state
    protected internal virtual void ChooseNextAttackState()
    {
        Debug.LogError("Boss choosing next attack state");

        string previousState = currentState.ToString();

        List<string> newRandomStates = new List<string>();

        foreach(string state in combatStates)
        {
            newRandomStates.Add(state);
        }

        if (newRandomStates.Contains(previousState))
        {
            newRandomStates.Remove(previousState);
        }

        string nextState = newRandomStates[UnityEngine.Random.Range(0, newRandomStates.Count)];

        SelectCombatState(nextState);
    }

    // Update is called once per frame
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

    //Boss Active (when boss is enabled)
    public virtual void BossActive()
    {
        onBossActive.Invoke();     //Boss Active event (closes boss arena doors, stops the music, etc.)
    }

    public virtual void BossInCombat()
    {
        BossHealth bossHealth = GetComponentInChildren<BossHealth>();

        if (bossHealth != null)
        {
            bossHealth.BossCanBeDamaged(true);
        }

        onBossInCombat.Invoke();    //Boss is fighting and can take damage

        SetPlayerTarget();
        ChooseNextAttackState();
    }

    protected internal virtual void CanMove(bool _bool)
    {
        aiLerp.canMove = _bool;
    }

    //Boss Defeated
    protected internal virtual void BossDefeated()
    {
        onBossDefeated.Invoke();
    }

    //Set player target
    public void SetPlayerTarget()
    {
        currentPlayerTarget = GameObject.Find("Player Robot(Clone)").transform;
    }

    //Set waypoint target
    public void SetWaypoint(Transform waypoint)
    {
        currentWaypointTarget = waypoint;

        aiDestinationSetter.target = currentWaypointTarget;
    }
}
