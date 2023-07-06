using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileRadar : MonoBehaviour
{
    HomingMissileBehavior_Old homingMissileBehavior;
    public Transform target;

    bool lostTrackOfTarget = false;

    void Start()
    {
        homingMissileBehavior = transform.parent.GetComponent<HomingMissileBehavior_Old>();

        if (homingMissileBehavior)
        {
            target = homingMissileBehavior.target;
            Debug.LogError("MISSILE GOT TARGET");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == target)
        {
            if (homingMissileBehavior && !lostTrackOfTarget)
            {
                lostTrackOfTarget = true;
                homingMissileBehavior.LostTrackOfTarget();
            }    
        }
    }
}
