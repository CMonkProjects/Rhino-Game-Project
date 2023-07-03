using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_SlotAttacker_MultiWeapon_Idle : EnemyState
{
    EnemyBehavior_SlotAttacker_MultiWeapon enemyBehavior_SlotAttacker_MultiWeapon;
    public EnemyState_SlotAttacker_MultiWeapon_Idle(EnemyBehavior_SlotAttacker_MultiWeapon enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_SlotAttacker_MultiWeapon = enemyBehavior;
    }

    public override void Tick()
    {
        if (enemyBehavior_SlotAttacker_MultiWeapon.currentPlayerTarget != null)
        {
            enemyBehavior_SlotAttacker_MultiWeapon.SwitchToNewState(typeof(EnemyState_SlotAttacker_MultiWeapon_AlertChasing));
        }
    }
}
