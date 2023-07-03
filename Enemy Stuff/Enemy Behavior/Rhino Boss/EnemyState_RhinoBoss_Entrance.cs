using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Rhino boss heads towards waypoint then starts boss fight
public class EnemyState_RhinoBoss_Entrance : EnemyState
{
    EnemyBehavior_RhinoBoss enemyBehavior_RhinoBoss;
    EnemyTurret_RhinoBoss enemyTurret;

    EnemyHealth_RhinoBoss enemyHealth_RhinoBoss;

    public EnemyState_RhinoBoss_Entrance(EnemyBehavior_RhinoBoss enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_RhinoBoss = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        enemyTurret = (EnemyTurret_RhinoBoss)enemyBehavior_RhinoBoss.enemyTurret;

        if (enemyTurret)
        {
            enemyTurret.SetAutoFire(false);
            enemyTurret.StopAllCoroutines();
        }

        //rhino boss can't be damaged yet
        enemyHealth_RhinoBoss = enemyBehavior_RhinoBoss.transform.GetChild(0).GetComponent<EnemyHealth_RhinoBoss>();

        if (enemyHealth_RhinoBoss)
        {
            enemyHealth_RhinoBoss.CanTakeDamage(false);
        }

        HeadToWaypoint();
    }

    public override void Tick()
    {
        //Boss starts combat when reached end of waypoint
        if (enemyBehavior_RhinoBoss.aiLerp.reachedEndOfPath)
        {
            if (enemyHealth_RhinoBoss)
            {
                enemyHealth_RhinoBoss.CanTakeDamage(true);
            }
            
            enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_ChooseNextAttack));
        }
    }

    void HeadToWaypoint()
    {
        enemyBehavior_RhinoBoss.aiDestinationSetter.target = enemyBehavior_RhinoBoss.waypointGroupEntrance;
    }

    public override void OnStateExit()
    {
        //Boss can now take damage
        if (enemyHealth_RhinoBoss)
        {
            enemyHealth_RhinoBoss.CanTakeDamage(true);
            Debug.LogError("BOSS CAN TAKE DAMAGE");
        }
    }
}
