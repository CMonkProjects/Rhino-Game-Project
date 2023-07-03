using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Here we pick the next attack state, it's chosen randomly but we never pick the same state twice in a row
public class EnemyState_RhinoBoss_ChooseNextAttack : EnemyState
{
    EnemyBehavior_RhinoBoss enemyBehavior_RhinoBoss;

    string previousAttack;

    List<string> combatStates = new List<string>() { "EnemyState_RhinoBoss_Blaster", "EnemyState_RhinoBoss_Shotgun", "EnemyState_RhinoBoss_Missile", "EnemyState_RhinoBoss_RapidFire" };

    public EnemyState_RhinoBoss_ChooseNextAttack(EnemyBehavior_RhinoBoss enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_RhinoBoss = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        previousAttack = enemyBehavior_RhinoBoss.previousState;

        PickNextAttack();
    }

    public override void Tick()
    {
        //do nothing
    }

    void PickNextAttack()
    {
        //Grab all the potential attack states we can use
        List<string> potentialAttackStates = new List<string>();

        foreach (string state in combatStates)
        {
            potentialAttackStates.Add(state);
        }

        //remove the previous state we just came from (so no attack repeats twice in a row)
        if (potentialAttackStates.Contains(previousAttack))
        {
            //previous state is EnemyState_RhinoBoss_Blaster   <- Example
            potentialAttackStates.Remove(previousAttack);
        }

        string nextAttack = potentialAttackStates[Random.Range(0, potentialAttackStates.Count)];

        switch (nextAttack)
        {
            case "EnemyState_RhinoBoss_Blaster":
                enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_Blaster));
                break;

            case "EnemyState_RhinoBoss_Shotgun":
                enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_Shotgun));
                break;

            case "EnemyState_RhinoBoss_Missile":
                enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_Missile));
                break;

            case "EnemyState_RhinoBoss_RapidFire":
                enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_RapidFire));
                break;
        }

        Debug.LogError("NEXT ATTACK STATE IS - " + nextAttack);
    }
}
