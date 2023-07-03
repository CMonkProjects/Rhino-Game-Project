using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBehavior_Static : EnemyBehavior
{
    public enum StartingState
    {
        Idle
    }

    //Chosen in the inspector
    public StartingState startingState;

    protected override void InitializeStates()
    {
        var states = new Dictionary<Type, EnemyState>()
        {
            {typeof(EnemyState_Static_Idle), new EnemyState_Static_Idle(this) }
        };

        SetStates(states);
        SelectStartingState(startingState);
    }

    protected virtual void SelectStartingState(StartingState startingState)
    {
        switch (startingState)
        {
            case StartingState.Idle:
                SwitchToNewState(typeof(EnemyState_Static_Idle));
                break;
        }
    }
}
