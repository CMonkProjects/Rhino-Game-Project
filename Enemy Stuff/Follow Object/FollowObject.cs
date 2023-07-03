using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform objectToFollow;

    void Start()
    {
        transform.parent = null;
    }

    void LateUpdate()
    {
        if (objectToFollow)
        {
            transform.position = new Vector2(objectToFollow.position.x, objectToFollow.position.y);
        }
    }
}
