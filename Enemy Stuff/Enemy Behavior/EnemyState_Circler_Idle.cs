using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Circler_Idle : EnemyState
{
    EnemyBehavior_Circler enemyBehavior_Circler;
    public EnemyState_Circler_Idle(EnemyBehavior_Circler enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Circler = enemyBehavior;
    }

    public override void Tick()
    {
        if (enemyBehavior_Circler.currentPlayerTarget != null)
        {
            enemyBehavior_Circler.SwitchToNewState(typeof(EnemyState_Circler_AlertChasing));
        }
    }
}
