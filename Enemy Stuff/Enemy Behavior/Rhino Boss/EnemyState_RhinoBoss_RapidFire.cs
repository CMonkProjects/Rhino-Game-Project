using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//boss finds random waypoint then launches rocket swarm
public class EnemyState_RhinoBoss_RapidFire : EnemyState
{
    EnemyBehavior_RhinoBoss enemyBehavior_RhinoBoss;
    EnemyTurret_RhinoBoss enemyTurret;

    bool startShootingOnce = true;

    int weaponIndex = 7;    //Rapid Fire

    public EnemyState_RhinoBoss_RapidFire(EnemyBehavior_RhinoBoss enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_RhinoBoss = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        enemyBehavior_RhinoBoss.aiDestinationSetter.target = null;
        enemyBehavior_RhinoBoss.aiLerp.SetPath(null);

        enemyTurret = (EnemyTurret_RhinoBoss)enemyBehavior_RhinoBoss.enemyTurret;

        //listen to the delegate
        enemyTurret.doneShooting += DoneShootingWeapon;

        if (enemyTurret)
        {
            enemyTurret.SwitchWeapon(weaponIndex);
            enemyTurret.SetAutoFire(false);
            enemyTurret.StopAllCoroutines();
        }

        enemyBehavior_RhinoBoss.aiDestinationSetter.target = null;

        startShootingOnce = false;

        FindFurthestWaypointFromPlayer();
    }

    public override void Tick()
    {
        if (enemyBehavior_RhinoBoss.aiLerp.reachedEndOfPath && enemyBehavior_RhinoBoss.aiLerp.remainingDistance <= 0f)
        {
            if (startShootingOnce && enemyTurret.canAttack)
            {
                enemyTurret.StartCoroutine(enemyTurret.StartShooting());
                startShootingOnce = false;
            }
        }
    }

    void FindFurthestWaypointFromPlayer()
    {
        //find the waypoint that is the furthest distance from the player and move to it

        enemyBehavior_RhinoBoss.aiDestinationSetter.target = null;

        List<Transform> waypointList = new List<Transform>();

        //make list of all available waypoints
        if (enemyBehavior_RhinoBoss.waypointGroupCenter != null)
        {
            foreach (Transform childWaypoint in enemyBehavior_RhinoBoss.waypointGroupCenter)
            {
                waypointList.Add(childWaypoint);
            }
        }

        float highestDistance = 0f;

        foreach (Transform childWaypoint in enemyBehavior_RhinoBoss.waypointGroupCenter)
        {
            float distance = Vector2.Distance(enemyBehavior_RhinoBoss.currentPlayerTarget.transform.position, childWaypoint.transform.position);

            //If this current waypoint is furthest from the player we move towards it
            if (distance > highestDistance)
            {
                highestDistance = distance;
                enemyBehavior_RhinoBoss.aiDestinationSetter.target = childWaypoint;
                startShootingOnce = true;
            }
        }
    }

    //check delegate for when weapon is finished firing
    void DoneShootingWeapon(int _weaponIndex)
    {
        if (_weaponIndex == weaponIndex)
        {
            Debug.LogError("BOSS IS DONE SHOOTING RAPID FIRE");
        }

        //Exit State
        enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_ChooseNextAttack));
    }

    public override void OnStateExit()
    {
        enemyTurret.doneShooting -= DoneShootingWeapon;
    }
}