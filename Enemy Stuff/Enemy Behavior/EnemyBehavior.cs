using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] public EnemyState currentState;

    public Transform currentPlayerTarget;

    [SerializeField] protected float tickRate = 0.5f;
    protected float tickTimer;

    [SerializeField] protected internal GameObject enemyTurretObject;
    [SerializeField] protected internal EnemyTurret enemyTurret;

    protected EnemyHealth enemyHealth;

    //A list (dictionary) of all the states our statemachine has available to it
    protected Dictionary<Type, EnemyState> availableStates;

    protected virtual void Start()
    {
        GetTurretStuff();
        enemyHealth = transform.GetChild(0).GetComponent<EnemyHealth>();
        InitializeStates();

        //Listen to delegates
        if (enemyHealth != null)
        {
            enemyHealth.onDying += OnDying;
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

    protected virtual void GetTurretStuff()
    {
        enemyTurretObject = transform.Find("Enemy_Turret").gameObject;

        if (enemyTurretObject != null)
        {
            enemyTurret = enemyTurretObject.GetComponent<EnemyTurret>();
            enemyTurretObject.transform.localRotation = Quaternion.identity;
        }
    }

    protected virtual void InitializeStates()
    {
        var states = new Dictionary<Type, EnemyState>()
        {
            //States go here
            //___Example____
            //{typeof(Enemy_State_Mobile_Idle), new Enemy_State_Mobile_Idle(this) }
        };

        SetStates(states);
    }

    protected virtual void SetStates(Dictionary<Type, EnemyState> states)
    {
        availableStates = states;
    }

    //call the current state's tick function every half second
    protected virtual void Update()
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

    public virtual void SwitchToNewState(Type newState)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = availableStates[newState];

        if (currentState != null)
            currentState.OnStateEnter();
    }

    //Player Detected
    public virtual void PlayerDetected(Transform _playerTarget)
    {
        currentPlayerTarget = _playerTarget;

        SetTurretTarget(currentPlayerTarget);
    }

    //Set turret target
    public virtual void SetTurretTarget(Transform _target)
    {
        if (enemyTurret != null)
        {
            enemyTurret.SetCurrentTarget(_target);
        }
    }

    //On Destroy
    public virtual void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.onDying -= OnDying;
        }
    }

    protected virtual void OnDying()
    {
        StartCoroutine(OnDyingCoroutine());
    }

    protected virtual IEnumerator OnDyingCoroutine()
    {
        yield return new WaitForEndOfFrame();
    }
}