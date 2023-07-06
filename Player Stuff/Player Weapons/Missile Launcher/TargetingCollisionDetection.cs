using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingCollisionDetection : MonoBehaviour
{
    public Transform playerTransform;
    public PlayerMissileTargeting playerMissileTargeting;

    void Awake()
    {
        transform.parent = null;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Transform target = collision.transform;

        if (playerMissileTargeting != null)
        {
            playerMissileTargeting.CollisionDetected(target);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Transform target = collision.transform;

        if (playerMissileTargeting != null)
        {
            playerMissileTargeting.CollisionExitDetected(target);
        }
    }
}
