using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseBehavior : MonoBehaviour
{
    Vector3 mousePosition;
    public float moveSpeed;

    void OnEnable()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;

        transform.position = mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;

        transform.position = mousePosition;
    }
}
