using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Sniper_Idle : EnemyState
{
    EnemyBehavior_Sniper enemyBehavior_Sniper;
    public EnemyState_Sniper_Idle(EnemyBehavior_Sniper enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Sniper = enemyBehavior;
    }

    public override void Tick()
    {
        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            enemyBehavior_Sniper.SwitchToNewState(typeof(EnemyState_Sniper_AlertChasing));
        }
    }
}
