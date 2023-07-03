using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Sniper_KeepDistance : EnemyState
{
    EnemyBehavior_Sniper enemyBehavior_Sniper;

    public EnemyState_Sniper_KeepDistance(EnemyBehavior_Sniper enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Sniper = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        enemyBehavior_Sniper.aiDestinationSetter.target = null;
        enemyBehavior_Sniper.aiLerp.SetPath(null);
        KeepDistanceFromPlayer();
    }

    public override void Tick()
    {
        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            //check the distance between player and sniper

            //if it's less than maxDistance then we run from the player, else we switch to alert chase again
            switch (CheckPlayerDistance())
            {
                case true:
                    AlertChase();
                    break;

                case false:
                    KeepDistanceFromPlayer();
                    break;
            }
        }
    }

    bool CheckPlayerDistance()
    {
        float distance = 0f;

        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            distance = Vector2.Distance(transform.position, enemyBehavior_Sniper.currentPlayerTarget.transform.position);

            //Debug.LogError("Sniper is - " + distance + " away from player");
        }

        if (distance > enemyBehavior_Sniper.maxDistanceToPlayer)
        {
            //Debug.LogError("Sniper is too far from player");
            return true;
        }
        else return false;
    }

    void AlertChase()
    {
        if (enemyBehavior_Sniper.currentPlayerTarget != null)
        {
            //Debug.LogError("Sniper distance is over max, changing state");
            enemyBehavior_Sniper.SwitchToNewState(typeof(EnemyState_Sniper_AlertChasing));
        }
    }

    void KeepDistanceFromPlayer()
    {
        //find the slot (out of 4) that is the farthest from the player so we can move towards it (and away from the player)

        enemyBehavior_Sniper.aiDestinationSetter.target = null;

        float highestDistance = 0f;

        foreach (Transform slot in enemyBehavior_Sniper.slotsKeepDistanceParent)
        {
            if (!enemyBehavior_Sniper.currentPlayerTarget)
            {
                break;
            }

            float distance = Vector2.Distance(enemyBehavior_Sniper.currentPlayerTarget.transform.position, slot.transform.position);

            //Debug.Log("Distance of " + slot.name + " is - " + distance);

            //If this current slot is furthest from the player we move towards it
            if (distance > highestDistance)
            {
                highestDistance = distance;
                enemyBehavior_Sniper.aiDestinationSetter.target = slot;
            }
        }
    }
}
