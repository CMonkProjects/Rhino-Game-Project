using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Boss_Entrance : EnemyState
{
    BossBehavior_TestBoss bossBehavior_testBoss;
    public EnemyState_Boss_Entrance(BossBehavior_TestBoss bossBehavior) : base(bossBehavior.gameObject)
    {
        bossBehavior_testBoss = bossBehavior;
    }

    public override void OnStateEnter()
    {
        //get the entrance waypoint reference then follow it
        SetEntranceWaypoint();
    }

    public override void Tick()
    {
        //At end of waypoint we enter the boss combat phase (boss combat state, boss combat unity event, can be damaged, etc.)
        if (bossBehavior_testBoss.aiLerp.reachedEndOfPath)
        {
            bossBehavior_testBoss.BossInCombat();
        }
    }

    void SetEntranceWaypoint()
    {
        if (bossBehavior_testBoss)
        {
            if (bossBehavior_testBoss.aiDestinationSetter)
            {
                bossBehavior_testBoss.SetWaypoint(bossBehavior_testBoss.waypointEntrance);
            }
        }
    }
}
