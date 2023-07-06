using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTargetLockOn : MonoBehaviour
{
    Animator animator;

    [SerializeField] string animBool_lockedOn = "LockedOn";
    [SerializeField] string animBool_targeting = "Targeting";
    [SerializeField] string animBool_lineOfSight = "LineOfSight";

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    //When enemy is being targeted
    internal void Animation_Targeting(bool _targeting)
    {
        if (animator)
        {
            animator.SetBool(animBool_targeting, _targeting);
            
            if (!_targeting)
            {
                animator.SetBool(animBool_lockedOn, false);
            }
        }
    }

    internal void Animation_LineOfSight(bool _lineOfSight)
    {
        if (animator)
        {
            animator.SetBool(animBool_lineOfSight, _lineOfSight);

            if (!_lineOfSight)
            {
                animator.SetBool(animBool_lockedOn, false);
            }
        }
    }

    //When enemy is locked-on (called when the locking-on animation is complete)
    internal void Animation_LockedOn()
    {
        EnemyObjectTargeting enemyObjectTargeting;

        enemyObjectTargeting = transform.parent.GetComponent<EnemyObjectTargeting>();

        if (enemyObjectTargeting)
        {
            enemyObjectTargeting.LockedOn(true);
        }
    }
}
