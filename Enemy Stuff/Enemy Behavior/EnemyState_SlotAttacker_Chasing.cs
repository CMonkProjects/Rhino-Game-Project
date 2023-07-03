using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_SlotAttacker_Chasing : EnemyState
{
    EnemyBehavior_SlotAttacker enemyBehavior_SlotAttacker;

    float tickerResetTime = 4f;
    float tickerTimer;

    public EnemyState_SlotAttacker_Chasing(EnemyBehavior_SlotAttacker enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_SlotAttacker = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        tickerTimer = tickerResetTime;
    }

    public override void Tick()
    {
        if (enemyBehavior_SlotAttacker.aiDestinationSetter.target == null)
        {
            if (enemyBehavior_SlotAttacker.currentPlayerTarget)
                PickAttackSlot();
        }

        if (tickerTimer > 0f)
        {
            tickerTimer -= 1f;
        }
        else ClearSlot();
    }

    public void PickAttackSlot()
    {
        var slotManager = enemyBehavior_SlotAttacker.currentPlayerTarget.GetComponent<SlotManager>();

        if (slotManager != null)
        {
            //If we don't have a position around the player then find the closest one
            if (enemyBehavior_SlotAttacker.slot == -1)
                enemyBehavior_SlotAttacker.slot = slotManager.ReserveSlot(gameObject);

            if (enemyBehavior_SlotAttacker.slot == -1)
                return;

            //Move to the slot to attack the player
            if (enemyBehavior_SlotAttacker.aiDestinationSetter != null)
            {
                var slotPosition = slotManager.GetSlotPosition(enemyBehavior_SlotAttacker.slot);

                enemyBehavior_SlotAttacker.aiDestinationSetter.target = slotPosition;
            }
        }
    }

    //Clear our enemy's chosen slot so they can pick a closer one if available
    public void ClearSlot()
    {
        enemyBehavior_SlotAttacker.ClearSlot();

        if (enemyBehavior_SlotAttacker.aiDestinationSetter != null)
            enemyBehavior_SlotAttacker.aiDestinationSetter.target = null;

        tickerTimer = tickerResetTime;
    }
}
