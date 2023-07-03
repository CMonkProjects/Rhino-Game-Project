using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;  //need this for AILerp and AIDestinationSetter
using System;

public class EnemyBehavior_SlotAttacker_MultiWeapon : EnemyBehavior_SlotAttacker
{
    protected override void InitializeStates()
    {
        var states = new Dictionary<Type, EnemyState>()
        {
            {typeof(EnemyState_SlotAttacker_MultiWeapon_Idle), new EnemyState_SlotAttacker_MultiWeapon_Idle(this) },
            {typeof(EnemyState_SlotAttacker_MultiWeapon_AlertChasing), new EnemyState_SlotAttacker_MultiWeapon_AlertChasing(this) },
            {typeof(EnemyState_SlotAttacker_MultiWeapon_Patrolling), new EnemyState_SlotAttacker_MultiWeapon_Patrolling(this) },
            {typeof(EnemyState_SlotAttacker_MultiWeapon_SpecialWeaponFiring), new EnemyState_SlotAttacker_MultiWeapon_SpecialWeaponFiring(this) }
        };

        SetStates(states);
        SelectStartingState(startingState);
    }

    public override void SelectStartingState(StartingState nextState)
    {
        switch (nextState)
        {
            case StartingState.AlertChasing:
                SwitchToNewState(typeof(EnemyState_SlotAttacker_MultiWeapon_AlertChasing));
                enemyRadar.EnemyAlerted();
                break;

            case StartingState.Idle:
                SwitchToNewState(typeof(EnemyState_SlotAttacker_MultiWeapon_Idle));
                break;

            case StartingState.Patrolling:
                SwitchToNewState(typeof(EnemyState_SlotAttacker_MultiWeapon_Patrolling));
                break;
        }
    }
}
