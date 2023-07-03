using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected EnemyBehavior enemyBehavior;
    //The enemy unit that created this behavior script
    protected GameObject gameObject;
    protected Transform transform;

    public EnemyState(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
    }

    //Every state needs the Tick method
    public abstract void Tick();

    //The enter and exit methods are optional
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
}
