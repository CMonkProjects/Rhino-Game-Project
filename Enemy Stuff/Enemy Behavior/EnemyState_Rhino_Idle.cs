using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Rhino_Idle : EnemyState
{
    EnemyBehavior_Rhino enemyBehavior_Rhino;
    public EnemyState_Rhino_Idle(EnemyBehavior_Rhino enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Rhino = enemyBehavior;
    }

    public override void Tick()
    {
        if (enemyBehavior_Rhino.currentPlayerTarget != null)
        {
            enemyBehavior_Rhino.SwitchToNewState(typeof(EnemyState_Rhino_AlertChasing));
        }
    }
}
