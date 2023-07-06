using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaTargetSetter : MonoBehaviour
{
    public PlayerWeaponPlasma playerWeaponPlasma;

    //current target (only has one at a time)
    Transform currentTarget;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (currentTarget != null)
        {
            return;
        }

        currentTarget = collision.transform;

        SetNewTarget(currentTarget);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform != currentTarget)
        {
            return;
        }

        currentTarget = null;

        SetNewTarget(currentTarget);
    }

    //Set new Target
    void SetNewTarget(Transform _newTarget)
    {
        if (playerWeaponPlasma != null)
        {
            playerWeaponPlasma.NewTarget(_newTarget);
            
        }
    }
}
