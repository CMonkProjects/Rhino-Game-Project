using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Sniper_Stop : EnemyState
{
    EnemyBehavior_Sniper enemyBehavior_Sniper;
    EnemyRadar enemyRadar;

    public EnemyState_Sniper_Stop(EnemyBehavior_Sniper enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Sniper = enemyBehavior;
        enemyRadar = enemyBehavior.gameObject.GetComponent<EnemyRadar>();
    }

    public override void OnStateEnter()
    {
        enemyBehavior_Sniper.aiDestinationSetter.target = null;
        enemyBehavior_Sniper.aiLerp.canMove = false;    //enemy stops immediately
        enemyBehavior_Sniper.aiLerp.SetPath(null);
    }

    public override void Tick()
    {
        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            //check the distance between player and sniper and react accordingly

            switch (CheckPlayerDistance())
            {
                case "Too Far":
                    AlertChase();
                    break;

                case "Too Close":
                    KeepDistance();
                    break;

                case "Just Right":
                    //Do nothing
                    break;
            }
        }
    }

    string CheckPlayerDistance()
    {
        float distance = 0f;

        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            distance = Vector2.Distance(transform.position, enemyBehavior_Sniper.currentPlayerTarget.transform.position);
        }

        if (distance > enemyBehavior_Sniper.maxDistanceToPlayer)
        {
            return "Too Far";
        }
        
        if (distance < enemyBehavior_Sniper.minDistanceToPlayer && enemyRadar.hasLineOfSight)
        {
            return "Too Close";
        }
        
        return "Just Right";
    }

    void AlertChase()
    {
        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            enemyBehavior_Sniper.SwitchToNewState(typeof(EnemyState_Sniper_AlertChasing));
        }
    }

    void KeepDistance()
    {
        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            enemyBehavior_Sniper.SwitchToNewState(typeof(EnemyState_Sniper_KeepDistance));
        }
    }

    public override void OnStateExit()
    {
        enemyBehavior_Sniper.aiLerp.canMove = true; //enemy can move again
        enemyBehavior_Sniper.aiLerp.SetPath(null);
    }
}
