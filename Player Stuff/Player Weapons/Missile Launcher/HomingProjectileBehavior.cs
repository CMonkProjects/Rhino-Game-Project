using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.theappguruz.com/blog/create-homing-missiles-in-game-unity-tutorial
public class HomingProjectileBehavior : MonoBehaviour
{
    [SerializeField] Transform target;

    Rigidbody2D rb;

    public float speed;
    public float rotationSpeed;

    public float homingTime;
    float currentTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        float percentage = currentTime / homingTime;

        rotationSpeed = Mathf.Lerp(rotationSpeed, 0, percentage);
    }

    void FixedUpdate()
    {
        RotateToTarget();
        
        rb.velocity = transform.up * speed * Time.deltaTime;
    }

    void RotateToTarget()
    {
        if (target != null) 
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotationSpeed * rotateAmount;
        }
    }
}
