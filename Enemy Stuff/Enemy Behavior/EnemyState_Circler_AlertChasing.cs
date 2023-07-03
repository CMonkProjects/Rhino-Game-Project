using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyState_Circler_AlertChasing : EnemyState
{
    EnemyBehavior_Circler enemyBehavior_Circler;
    AIDestinationSetter aiDestinationSetter;
    AILerp aiLerp;
    SlotManagerCorners slotManagerCorners;

    int currentSlot = -1;

    public EnemyState_Circler_AlertChasing(EnemyBehavior_Circler enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Circler = enemyBehavior;
        aiDestinationSetter = enemyBehavior.gameObject.GetComponent<AIDestinationSetter>();
        aiLerp = enemyBehavior.gameObject.GetComponent<AILerp>();
    }

    public override void Tick()
    {
        if (enemyBehavior_Circler.currentPlayerTarget && aiDestinationSetter.target == null)
        {
            StartCirclingPlayer();
            return;
        }

        if (aiLerp.remainingDistance < 0.16f)
            CirclePlayer();
    }

    void StartCirclingPlayer()
    {
        slotManagerCorners = enemyBehavior_Circler.currentPlayerTarget.GetComponent<SlotManagerCorners>();

        if (!slotManagerCorners)
            return;

        currentSlot = slotManagerCorners.FindClosestSlot(gameObject);

        var point = slotManagerCorners.GetCornerSlotPosition(currentSlot);

        aiDestinationSetter.target = point;
    }

    void CirclePlayer()
    {
        if (!slotManagerCorners)
            return;

        currentSlot++;

        if (currentSlot > slotManagerCorners.cornerSlots.Count - 1)
            currentSlot = 0;

        var point = slotManagerCorners.GetCornerSlotPosition(currentSlot);

        aiDestinationSetter.target = point;
    }
}
