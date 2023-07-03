using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Boss_Shotgun : EnemyState
{
    BossBehavior_TestBoss bossBehavior_testBoss;
    BossTurret bossTurret;
    public EnemyState_Boss_Shotgun(BossBehavior_TestBoss bossBehavior) : base(bossBehavior.gameObject)
    {
        bossBehavior_testBoss = bossBehavior;
        bossTurret = bossBehavior_testBoss.enemyBossTurret;
    }

    public override void OnStateEnter()
    {
        //Set the turret weapon to shotgun
        bossTurret.SwitchToNewTurretWeapon(1);

        //boss doesn't move
        bossBehavior_testBoss.CanMove(false);

        //Tell turret to track player
        bossTurret.SetNewTarget(bossBehavior_testBoss.currentPlayerTarget);
        bossTurret.RotateTowardsTarget(true);
    }

    public override void Tick()
    {
        //Check if turret is done shooting, if so we exit this state
        if (bossTurret.DoneShooting())
        {
            bossBehavior_testBoss.ChooseNextAttackState();
        }
    }

    public override void OnStateExit()
    {
        bossBehavior_testBoss.CanMove(true);
    }
}
