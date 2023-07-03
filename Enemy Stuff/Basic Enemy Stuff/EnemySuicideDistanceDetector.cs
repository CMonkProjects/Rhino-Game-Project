using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySuicideDistanceDetector : EnemyRhinoDistanceDetector
{
    protected override void CheckPlayerDistance()
    {
        if (!canCheckDistance)
            return;

        float distance = 0f;

        if (enemyBehavior.currentPlayerTarget)
        {
            distance = Vector2.Distance(transform.position, enemyBehavior.currentPlayerTarget.transform.position);
        }

        if (distance <= alertDistance)
        {
            GameEvents.current.EnemySuicideNear();
            canCheckDistance = false;
        }
    }
}
