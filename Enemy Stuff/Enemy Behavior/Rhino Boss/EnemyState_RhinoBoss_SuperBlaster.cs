using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
//Boss chases after player while firing its blasters
public class EnemyState_RhinoBoss_SuperBlaster : EnemyState
{
    EnemyBehavior_RhinoBoss enemyBehavior_RhinoBoss;
    EnemyTurret_RhinoBoss enemyTurret;

    float tickerResetTime = 4f;
    float tickerTimer;

    int weaponIndex = 4;

    public EnemyState_RhinoBoss_SuperBlaster(EnemyBehavior_RhinoBoss enemyBehavior) : base(enemyBehavior.gameObject)
    {
        enemyBehavior_RhinoBoss = enemyBehavior;
    }

    public override void OnStateEnter()
    {
        enemyTurret = (EnemyTurret_RhinoBoss)enemyBehavior_RhinoBoss.enemyTurret;

        //listen to the delegate
        enemyTurret.doneShooting += DoneShootingWeapon;

        if (enemyTurret)
        {
            enemyTurret.SwitchWeapon(weaponIndex);    //switch to blaster
            enemyTurret.SetAutoFire(true);
            enemyTurret.StopAllCoroutines();
        }

        //Clear the destination target
        enemyBehavior_RhinoBoss.aiDestinationSetter.target = null;
    }

    public override void Tick()
    {
        //move to player slot
        if (enemyBehavior_RhinoBoss.aiDestinationSetter.target == null)
        {
            if (enemyBehavior_RhinoBoss.currentPlayerTarget)
                PickAttackSlot();
        }

        if (tickerTimer > 0f)
        {
            tickerTimer -= 1f;
        }
    }

    void PickAttackSlot()
    {
        var slotManager = enemyBehavior_RhinoBoss.currentPlayerTarget.GetComponent<SlotManager>();

        if (slotManager != null)
        {
            //If we don't have a position around the player then find the closest one
            if (enemyBehavior_RhinoBoss.slot == -1)
                enemyBehavior_RhinoBoss.slot = slotManager.ReserveSlot(gameObject);

            if (enemyBehavior_RhinoBoss.slot == -1)
                return;

            //Move to the slot to attack the player
            if (enemyBehavior_RhinoBoss.aiDestinationSetter != null)
            {
                var slotPosition = slotManager.GetSlotPosition(enemyBehavior_RhinoBoss.slot);

                enemyBehavior_RhinoBoss.aiDestinationSetter.target = slotPosition;
            }
        }
    }

    void ClearSlot()
    {
        enemyBehavior_RhinoBoss.ClearSlot();

        if (enemyBehavior_RhinoBoss.aiDestinationSetter != null)
            enemyBehavior_RhinoBoss.aiDestinationSetter.target = null;

        tickerTimer = tickerResetTime;
    }

    //check delegate for when weapon is finished firing
    void DoneShootingWeapon(int _weaponIndex)
    {
        if (_weaponIndex == weaponIndex)
        {
            Debug.LogError("BOSS IS DONE RAPIDLY FIRING");
        }

        //Exit State
        enemyBehavior_RhinoBoss.SwitchToNewState(typeof(EnemyState_RhinoBoss_ChooseNextAttackFinal));
    }

    public override void OnStateExit()
    {
        ClearSlot();

        enemyTurret.doneShooting -= DoneShootingWeapon;
    }
}
