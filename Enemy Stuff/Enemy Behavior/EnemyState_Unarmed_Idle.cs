using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Unarmed_Idle : EnemyState
{
    EnemyBehavior_Unarmed enemyBehavior_Unarmed;

    public EnemyState_Unarmed_Idle(EnemyBehavior_Unarmed enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Unarmed = enemyBehavior;
    }

    public override void Tick()
    {
        
    }
}
