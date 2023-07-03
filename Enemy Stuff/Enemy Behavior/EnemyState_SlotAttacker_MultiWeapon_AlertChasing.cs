using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_SlotAttacker_MultiWeapon_AlertChasing : EnemyState
{
    EnemyBehavior_SlotAttacker_MultiWeapon enemyBehavior_SlotAttacker_MultiWeapon;

    float tickerResetTime = 8f;
    float tickerTimer;

    public EnemyState_SlotAttacker_MultiWeapon_AlertChasing(EnemyBehavior_SlotAttacker_MultiWeapon enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_SlotAttacker_MultiWeapon = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        tickerTimer = tickerResetTime;
    }

    public override void Tick()
    {
        if (enemyBehavior_SlotAttacker_MultiWeapon.aiDestinationSetter.target == null)
        {
            if (enemyBehavior_SlotAttacker_MultiWeapon.currentPlayerTarget)
                PickAttackSlot();
        }

        if (tickerTimer > 0f)
        {
            tickerTimer -= 1f;
        }
        else DoRandomAction();

        //Timer to fire secondary
        /*if (CheckDistanceToPlayer())
        {
            randomTickerTimer -= 1f;

            if (randomTickerTimer <= 0f)
                DoRandomAction();
        }*/
        //random variation to timer
    }

    public void PickAttackSlot()
    {
        var slotManager = enemyBehavior_SlotAttacker_MultiWeapon.currentPlayerTarget.GetComponent<SlotManager>();

        if (slotManager != null)
        {
            //If we don't have a position around the player then find the closest one
            if (enemyBehavior_SlotAttacker_MultiWeapon.slot == -1)
                enemyBehavior_SlotAttacker_MultiWeapon.slot = slotManager.ReserveSlot(gameObject);

            //If we failed to find a slot we try again
            if (enemyBehavior_SlotAttacker_MultiWeapon.slot == -1)
                return;

            //Move to the slot to attack the player
            if (enemyBehavior_SlotAttacker_MultiWeapon.aiDestinationSetter != null)
            {
                var slotPosition = slotManager.GetSlotPosition(enemyBehavior_SlotAttacker_MultiWeapon.slot);

                enemyBehavior_SlotAttacker_MultiWeapon.aiDestinationSetter.target = slotPosition;
            }
        }
    }

    void DoRandomAction()
    {
        int randomNumber = Random.Range(0, 2);

        switch (randomNumber)
        {
            case 0:
                ClearSlot();
                break;

            case 1:
                enemyBehavior_SlotAttacker_MultiWeapon.SwitchToNewState(typeof(EnemyState_SlotAttacker_MultiWeapon_SpecialWeaponFiring));
                break;
        }
    }

    //Clear our enemy's chosen slot so they can pick a closer one if available
    void ClearSlot()
    {
        enemyBehavior_SlotAttacker_MultiWeapon.ClearSlot();

        if (enemyBehavior_SlotAttacker_MultiWeapon.aiDestinationSetter != null)
            enemyBehavior_SlotAttacker_MultiWeapon.aiDestinationSetter.target = null;

        tickerTimer = tickerResetTime;
    }
}
