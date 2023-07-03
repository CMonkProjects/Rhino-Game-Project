using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_RhinoBoss_ChooseNextAttackFinal : EnemyState
{
    EnemyBehavior_RhinoBoss enemyBehavior_RhinoBoss;

    string previousAttack;

    List<string> combatStates = new List<string>() { "EnemyState_RhinoBoss_SuperBlaster", "EnemyState_RhinoBoss_SuperShotgun", "EnemyState_RhinoBoss_SuperPlasma", "EnemyState_RhinoBoss_SuperRockets" };

    public EnemyState_RhinoBoss_ChooseNextAttackFinal(EnemyBehavior_RhinoBoss enemyBehavior) : base(enemyBehavior.gameObject)
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
            //previous state is EnemyState_RhinoBoss_SuperBlaster   <- Example
            potentialAttackStates.Remove(previousAttack);
        }

        string nextAttack = potentialAttackStates[Random.Range(0, potentialAttackStates.Count)];

        switch (nextAttack)
        {
            case "EnemyState_RhinoBoss_SuperBlaster":
                enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_SuperBlaster));
                break;

            case "EnemyState_RhinoBoss_SuperShotgun":
                enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_SuperShotgun));
                break;

            case "EnemyState_RhinoBoss_SuperPlasma":
                enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_SuperPlasma));
                break;

            case "EnemyState_RhinoBoss_SuperRockets":
                enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_SuperRockets));
                break;
        }

        Debug.LogError("NEXT ATTACK STATE IS - " + nextAttack);
    }
}
