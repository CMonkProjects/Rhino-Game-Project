using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Mobile_Idle : EnemyState
{
    EnemyBehavior_Mobile enemyBehavior_Mobile;
    public EnemyState_Mobile_Idle(EnemyBehavior_Mobile enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Mobile = enemyBehavior;
    }

    public override void Tick()
    {
        if (enemyBehavior_Mobile.currentPlayerTarget != null)
        {
            enemyBehavior_Mobile.SwitchToNewState(typeof(EnemyState_Mobile_AlertChasing));
        }
    }
}
