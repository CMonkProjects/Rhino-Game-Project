using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyState_Rhino_AlertChasing : EnemyState
{
    EnemyBehavior_Rhino enemyBehavior_Rhino;
    AIDestinationSetter aiDestinationSetter;
    AILerp aiLerp;
    EnemyRadar enemyRadar;
    SlotManagerCorners slotManagerCorners;

    int currentSlot = -1;

    float combatDistance = 2f;

    float randomTickerResetTime = 10f;  //Really it's 5 seconds, since the ticker counts down every Tick() or half-second
    float randomTickerTimer;

    public EnemyState_Rhino_AlertChasing(EnemyBehavior_Rhino enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_Rhino = enemyBehavior;
        aiDestinationSetter = enemyBehavior.gameObject.GetComponent<AIDestinationSetter>();
        aiLerp = enemyBehavior.gameObject.GetComponent<AILerp>();
        enemyRadar = enemyBehavior.gameObject.GetComponent<EnemyRadar>();
    }

    public override void OnStateEnter()
    {
        randomTickerTimer = randomTickerResetTime;
    }

    public override void Tick()
    {
        if (aiDestinationSetter.target == null)
        {
            StartCirclingPlayer();
            return;
        }

        //check distance, and when timer allows, to do a random action
        if (CheckDistanceToPlayer())
        {
            randomTickerTimer -= 1f;

            if (randomTickerTimer <= 0f)
                DoRandomAction();
        }

        if (aiLerp.remainingDistance < 0.16f)
            CirclePlayer(enemyBehavior_Rhino.clockwise);
    }

    void StartCirclingPlayer()
    {
        slotManagerCorners = enemyBehavior_Rhino.currentPlayerTarget.GetComponent<SlotManagerCorners>();

        if (!slotManagerCorners)
            return;

        currentSlot = slotManagerCorners.FindClosestSlot(gameObject);

        var point = slotManagerCorners.GetCornerSlotPosition(currentSlot);

        aiDestinationSetter.target = point;
    }

    void CirclePlayer(bool _clockwise)
    {
        if (!slotManagerCorners)
            return;

        switch (_clockwise)
        {
            //true--------------------------------------------
            case true:
                currentSlot++;

                if (currentSlot > slotManagerCorners.cornerSlots.Count - 1)
                    currentSlot = 0;

                var point = slotManagerCorners.GetCornerSlotPosition(currentSlot);

                aiDestinationSetter.target = point;
                break;

            //false-------------------------------------------
            case false:
                currentSlot--;

                if (currentSlot < 0)
                    currentSlot = slotManagerCorners.cornerSlots.Count - 1;

                var _point = slotManagerCorners.GetCornerSlotPosition(currentSlot);

                aiDestinationSetter.target = _point;
                break;
        }
        
    }

    bool CheckDistanceToPlayer()
    {
        float distance = Vector2.Distance(transform.position, enemyBehavior_Rhino.currentPlayerTarget.transform.position);

        if (distance <= combatDistance && enemyRadar.hasLineOfSight)
        {
            return true;
        }
        else return false;
    }

    void DoRandomAction()
    {
        int randomNumber = Random.Range(0, 3);

        //Debug.LogError("DOING RANDOM ACTION, RANDOM NUMBER IS - " + randomNumber);

        if (enemyBehavior_Rhino.currentPlayerTarget != null)
        {
            switch (randomNumber)
            {
                case 0:
                    //Stop to fire special weapon
                    enemyBehavior_Rhino.SwitchToNewState(typeof(EnemyState_Rhino_Stopped));
                    break;

                case 1:
                    //Reverse direction
                    enemyBehavior_Rhino.ClockwiseSwitch();
                    break;

                case 2:
                    //Continue in current direction
                    break;
            }
        }

        randomTickerTimer = randomTickerResetTime;
    }
}