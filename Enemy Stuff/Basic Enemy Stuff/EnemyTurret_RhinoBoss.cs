using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret_RhinoBoss : EnemyTurret_MultiWeapon
{
    [SerializeField] EnemyHealth_RhinoBoss enemyHealth_RhinoBoss;

    //Coroutine shootingCoroutine;

    void Start()
    {
        //listen to the delegate
        enemyHealth_RhinoBoss.lowOnHealth += StopShootingTemporarily;
    }

    //it'll stop the turret from firing briefly until the new final attack state can enable it again
    void StopShootingTemporarily()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
        }
    }
}
