using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//Don't use anymore
//https://arongranberg.com/astar/docs/wander.html
public class EnemyState_Random_AlertChasing : EnemyState
{
    EnemyBehavior_Rhino enemyBehavior_Random;
    EnemyRadar enemyRadar;
    //IAstarAI ai;

    SlotManagerCorners slotManagerCorners;

    int currentSlot = 0;

    public EnemyState_Random_AlertChasing(EnemyBehavior_Rhino enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Random = enemyBehavior;
        enemyRadar = enemyBehavior.gameObject.GetComponent<EnemyRadar>();
        //ai = enemyBehavior_Random.ai;
    }

    public override void Tick()
    {
        //if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))  <- For some reason this isn't working anymore
        /*if (!ai.hasPath || ai.reachedEndOfPath)
        {
            Debug.LogError("Reached destination, picking new one");
            ai.destination = PickNextPoint();
            ai.SearchPath();
        }*/
    }

    Vector3 PickNextPoint()
    {
        slotManagerCorners = enemyBehavior_Random.currentPlayerTarget.GetComponent<SlotManagerCorners>();

        var point = slotManagerCorners.GetCornerSlotPosition(currentSlot).transform.position + Random.insideUnitSphere * 0.16f;
        point.z = 0f;

        Debug.LogError("Current Slot Index is: " + currentSlot);

        //increment the currentSlot for next time
        currentSlot++;

        if (currentSlot >= slotManagerCorners.cornerSlots.Count)
            currentSlot = 0;

        return point;
    }

    Vector3 PickRandomPoint()
    {
        var radius = 0.16f;

        var point = enemyBehavior_Random.currentPlayerTarget.transform.position + Random.insideUnitSphere * radius;

        point.z = 0f;

        return point;
    }
}
