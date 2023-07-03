using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileBehaviorBoss : HomingMissileBehavior_Old
{
    protected override void Start()
    {
        target = GameObject.Find("Player Robot(Clone)").transform;

        base.Start();
    }
}
