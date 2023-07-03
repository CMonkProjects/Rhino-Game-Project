using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Sniper_AlertChasing : EnemyState
{
    EnemyBehavior_Sniper enemyBehavior_Sniper;
    EnemyRadar enemyRadar;

    public EnemyState_Sniper_AlertChasing(EnemyBehavior_Sniper enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Sniper = enemyBehavior;
        enemyRadar = enemyBehavior.gameObject.GetComponent<EnemyRadar>();
    }

    public override void OnStateEnter()
    {
        StartChasingPlayer();
    }

    public override void Tick()
    {
        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            if (enemyBehavior_Sniper.aiDestinationSetter.target != enemyBehavior_Sniper.currentPlayerTarget)
            {
                StartChasingPlayer();
            }

            //check the distance between player and sniper

            switch (CheckPlayerDistance())
            {
                case "Chase Player":
                    //No change in behavior
                    break;

                case "Too Close":
                    KeepDistance();
                    break;

                case "Stay Put":
                    Stop();
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

        //Check distance to player to see if we should stop
        if (distance <= enemyBehavior_Sniper.stoppingDistanceToPlayer && distance > enemyBehavior_Sniper.minDistanceToPlayer && enemyRadar.hasLineOfSight)
        {
            return "Stay Put";
        }

        //Check distance to player to see if we're too close
        if (distance < enemyBehavior_Sniper.minDistanceToPlayer && enemyRadar.hasLineOfSight)
        {
            return "Too Close";
        }

        return "Chase Player";
    }

    public void StartChasingPlayer()
    {
        if (enemyBehavior_Sniper.currentPlayerTarget == null)
        {
            return;
        }

        if (enemyBehavior_Sniper.aiDestinationSetter != null)
        {
            enemyBehavior_Sniper.aiDestinationSetter.target = enemyBehavior_Sniper.currentPlayerTarget;
        }
    }

    void KeepDistance()
    {
        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            enemyBehavior_Sniper.SwitchToNewState(typeof(EnemyState_Sniper_KeepDistance));
        }
    }

    void Stop()
    {
        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            enemyBehavior_Sniper.SwitchToNewState(typeof(EnemyState_Sniper_Stop));
        }
    }
}
