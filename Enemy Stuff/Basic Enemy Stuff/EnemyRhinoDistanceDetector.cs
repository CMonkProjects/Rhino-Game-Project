using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRhinoDistanceDetector : MonoBehaviour
{
    protected EnemyBehavior enemyBehavior;

    public float timerMax;
    protected float timer = 0f;

    public float alertDistance;
    public bool canCheckDistance = true;

    protected virtual void OnEnable()
    {
        timer = timerMax;
    }

    protected virtual void Awake()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!canCheckDistance)
            return;

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            CheckPlayerDistance();
        }

    }

    protected virtual void CheckPlayerDistance()
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
            GameEvents.current.EnemyRhinoNear();
            canCheckDistance = false;
        }
    }
}
