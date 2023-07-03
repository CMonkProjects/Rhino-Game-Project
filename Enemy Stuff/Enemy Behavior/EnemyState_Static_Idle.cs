using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Static_Idle : EnemyState
{
    EnemyBehavior_Static enemyBehavior_Static;
    public EnemyState_Static_Idle(EnemyBehavior_Static enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Static = enemyBehavior;
    }

    public override void Tick()
    {
        if (enemyBehavior_Static.currentPlayerTarget != null)
        {

        }
    }
}
