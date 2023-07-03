using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;  //<--- Need this for referencing Pathfinding scripts

//This script tells the enemy unit what to do (patrol, chase player, sit idle, etc.)
public class EnemyAI : MonoBehaviour
{
    public Transform currentPathTarget;
    public Transform currentPlayerTarget;
    public Transform currentPatrolPathGroup;   //a parent object with child paths to loop through
    public List<Transform> currentPathList = new List<Transform>();
    [SerializeField] int currentPathIndex = 0;

    AIDestinationSetter aiDestinationSetter;

    AILerp aiLerp;

    float tickTime = 0.5f;
    float tickTimer = 0f;

    //All three of these states can detect the player, it just determines what the enemy is doing movement wise
    public enum EnemyState
    {
        //Idle,   //just sitting around
        Patrolling, //moving from waypoint to waypoint
        Attacking  //chasing after player
        //Searching   //Lost the player recently (maybe have enemy pick a new patrol point, such as the closest one in the area)
    }

    public EnemyState state = EnemyState.Patrolling;

    // Start is called before the first frame update
    void Start()
    {
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiLerp = GetComponent<EnemyAILerp>();

        GetPatrolWaypointsList();
    }

    void GetPatrolWaypointsList()
    {
        if (currentPatrolPathGroup == null)
        {
            Debug.LogWarning("Enemy doesn't have patrol group list");
            return;
        }

        foreach(Transform childWaypoint in currentPatrolPathGroup)
        {
            currentPathList.Add(childWaypoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tickTimer < tickTime)
        {
            tickTimer += Time.deltaTime;

            if (tickTimer >= tickTime)
            {
                BehaviorStateTick();
                tickTimer = 0f;
            }
        }
    }

    public void ChangeBehaviorState(EnemyState _enemyState)
    {
        state = _enemyState;

        switch (state)
        {
            //If enemy is unalert and following waypoints
            case EnemyState.Patrolling:



                break;

            //If enemy is alert and sees the player
            case EnemyState.Attacking:

                StartChasingPlayer();

                break;
        }
    }

    public void BehaviorStateTick()
    {
        //This state is called every second or so

        switch (state)
        {
            case EnemyState.Patrolling:

                if (currentPlayerTarget != null)
                {
                    Debug.Log("ENEMY HAS SPOTTED PLAYER");
                    ChangeBehaviorState(EnemyState.Attacking);
                    return;
                }

                //Go to next target path in path pool
                if (aiLerp.reachedEndOfPath)
                {
                    //FIND NEXT PATH IN GROUP
                    //SetNextPatrolPathTarget();
                }
                break;

            case EnemyState.Attacking:
                //If enemy lost track of the player we go to searching state
                if (currentPlayerTarget == null)
                {
                    Debug.Log("ENEMY LOST TRACK OF PLAYER");

                    //Enemy moves to player's last known location (enemy is following his radar ping)
                    if (aiDestinationSetter != null)
                    {
                        aiDestinationSetter.target = currentPlayerTarget;
                    }

                    return;
                }

                //If enemy has a turret (grandchild probably), then tell it to wake up if player is in weapon range
                break;
        }
    }
    

    //Patroling State Functions-------------------------------

    //If the enemy has reached the end of a patrol path
    public void EndOfPathReached()
    {
        switch (state)
        {
            case EnemyState.Patrolling:

                if (currentPathTarget == null)
                {
                    currentPathTarget = aiDestinationSetter.target;
                }

                Debug.Log("ENEMY HAS REACHED END OF PATH - " + currentPathTarget.name);

                break;

            case EnemyState.Attacking:

                

                ReturnToCurrentPatrolPath();

                break;

        }
    }

    public void SetNextPatrolPathTarget()
    {
        currentPathTarget = null;

        if (aiDestinationSetter != null)
        {
            if (currentPathIndex < currentPathList.Count - 1)
            {
                currentPathIndex++;
            }
            else
            {
                currentPathIndex = 0;
            }

            currentPathTarget = currentPathList[currentPathIndex];

            aiDestinationSetter.target = currentPathTarget;
        }
    }

    //Attacking State Functions------------------------------
    public void PlayedDetected(Transform _playerTarget)
    {
        currentPlayerTarget = _playerTarget;
    }

    public void StartChasingPlayer()
    {
        if (currentPlayerTarget != null)
        {
            //Chase after player
            if (aiDestinationSetter != null)
            {
                aiDestinationSetter.target = currentPlayerTarget;
            }
        }
    }

    public void ReturnToCurrentPatrolPath()
    {
        Debug.Log("Enemy is returning to current path target");

        if (aiDestinationSetter != null)
        {
            currentPathTarget = currentPathList[currentPathIndex];

            aiDestinationSetter.target = currentPathTarget;
        }

        ChangeBehaviorState(EnemyState.Patrolling);
    }

    //Tell turret to start shooting at player
}