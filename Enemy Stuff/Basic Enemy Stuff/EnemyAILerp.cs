using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAILerp : AILerp
{
    EnemyAI enemyAI;

    protected override void Awake()
    {
        base.Awake();

        enemyAI = GetComponent<EnemyAI>();
    }
    public override void OnTargetReached()
    {
        base.OnTargetReached();

        if (reachedEndOfPath)
        {
            TargetPathReached();
        }
    }

    void TargetPathReached()
    {
        if (enemyAI != null)
        {
            enemyAI.EndOfPathReached();
        }
    }
}
