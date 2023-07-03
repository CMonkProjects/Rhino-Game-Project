using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_SlotAttacker_Idle : EnemyState
{
    EnemyBehavior_SlotAttacker enemyBehavior_SlotAttacker;
    public EnemyState_SlotAttacker_Idle(EnemyBehavior_SlotAttacker enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_SlotAttacker = enemyBehavior;
    }

    public override void Tick()
    {
        if (enemyBehavior_SlotAttacker.currentPlayerTarget != null)
        {
            enemyBehavior_SlotAttacker.SwitchToNewState(typeof(EnemyState_SlotAttacker_Chasing));
        }
    }
}
