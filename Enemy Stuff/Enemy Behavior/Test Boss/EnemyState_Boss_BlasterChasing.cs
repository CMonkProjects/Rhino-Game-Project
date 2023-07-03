using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Boss_BlasterChasing : EnemyState
{
    BossBehavior_TestBoss bossBehavior_testBoss;
    BossTurret bossTurret;

    float minDistanceToPlayer = 0.75f;

    public EnemyState_Boss_BlasterChasing(BossBehavior_TestBoss bossBehavior) : base(bossBehavior.gameObject)
    {
        bossBehavior_testBoss = bossBehavior;
        bossTurret = bossBehavior_testBoss.enemyBossTurret;
    }

    public override void OnStateEnter()
    {
        //Set the turret weapon to blaster
        bossTurret.SwitchToNewTurretWeapon(0);

        //Tell turret to track player
        bossTurret.SetNewTarget(bossBehavior_testBoss.currentPlayerTarget);
        bossTurret.RotateTowardsTarget(true);

        //Tell boss to chase player
        StartChasingPlayer();
    }

    public override void Tick()
    {
        //check if boss is not too close to player, if so it moves (instead of switching to a stopping state)
        float distance = Vector2.Distance(transform.position, bossBehavior_testBoss.currentPlayerTarget.transform.position);

        if (distance < minDistanceToPlayer)
        {
            bossBehavior_testBoss.CanMove(false);
        }
        else bossBehavior_testBoss.CanMove(true);

        //Check if turret is done shooting, if so we exit this state
        if (bossTurret.DoneShooting())
        {
            bossBehavior_testBoss.ChooseNextAttackState();
        }
    }

    public override void OnStateExit()
    {
        StopChasingPlayer();
    }

    public void StartChasingPlayer()
    {
        if (bossBehavior_testBoss.currentPlayerTarget == null)
        {
            //find player
            return;
        }

        if (bossBehavior_testBoss.currentPlayerTarget != null)
        {
            if (bossBehavior_testBoss.aiDestinationSetter != null)
            {
                bossBehavior_testBoss.aiDestinationSetter.target = bossBehavior_testBoss.currentPlayerTarget;
            }
        }
    }

    public void StopChasingPlayer()
    {
        if (bossBehavior_testBoss.aiDestinationSetter != null)
        {
            bossBehavior_testBoss.aiDestinationSetter.target = null;
        }
    }
}
