using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Boss_Missile : EnemyState
{
    BossBehavior_TestBoss bossBehavior_testBoss;
    BossTurret bossTurret;
    Transform missileWaypointGroupParent;

    bool turretStartedShooting;

    public EnemyState_Boss_Missile(BossBehavior_TestBoss bossBehavior) : base(bossBehavior.gameObject)
    {
        bossBehavior_testBoss = bossBehavior;
        bossTurret = bossBehavior_testBoss.enemyBossTurret;
        missileWaypointGroupParent = bossBehavior_testBoss.missileWaypointGroupParent;
    }

    public override void OnStateEnter()
    {
        bossBehavior_testBoss.CanMove(true);
        turretStartedShooting = false;

        //Set the turret weapon to missiles
        bossTurret.SwitchToNewTurretWeapon(2);
        FindRandomWaypoint();
    }

    public override void Tick()
    {
        //if our boss reached the end of the path we start shooting
        if (bossBehavior_testBoss.aiLerp.reachedEndOfPath && !turretStartedShooting)
        {
            StartShooting();
        }

        if (turretStartedShooting)
        {
            //Check if turret is done shooting, if so we exit this state
            if (bossTurret.DoneShooting())
            {
                bossBehavior_testBoss.ChooseNextAttackState();
            }
        }
    }

    public void FindRandomWaypoint()
    {
        List<Transform> waypointList = new List<Transform>();

        //make list of all available waypoints
        if (missileWaypointGroupParent != null)
        {
            foreach (Transform childWaypoint in missileWaypointGroupParent)
            {
                waypointList.Add(childWaypoint);
            }
        }

        //find a random waypoint
        int randomIndex = UnityEngine.Random.Range(0, waypointList.Count);

        Transform nextWaypoint = waypointList[randomIndex];

        MoveToWaypoint(nextWaypoint);
    }

    public void MoveToWaypoint(Transform waypoint)
    {
        //once boss moved to waypoint it can begin launching missiles
        if (bossBehavior_testBoss.aiDestinationSetter)
        {
            bossBehavior_testBoss.SetWaypoint(waypoint);
        }
    }

    public void StartShooting()
    {
        //Tell turret to track player
        bossTurret.SetNewTarget(bossBehavior_testBoss.currentPlayerTarget);
        bossTurret.RotateTowardsTarget(true);

        turretStartedShooting = true;
    }
}
