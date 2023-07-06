using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileBehavior_Old : MonoBehaviour
{
    public Transform target;

    float currentSpeed;
    public float maxSpeed;
    public float startingSpeed;
    public float increaseSpeedRate;

    public float rotateSpeed = 10f;

    Rigidbody2D rb;
    Bullet bullet;

    public float homingTimer = 5f;
    float currentTimer;

    bool isHoming = true;
    bool isFlashing = false;

    //Materials
    protected Material matYellow;
    protected Material matDefault;
    protected SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        matYellow = Resources.Load("YellowFlash", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;
        bullet = GetComponent<Bullet>();

        currentTimer = homingTimer;

        currentSpeed = startingSpeed;
    }

    // Update is called once per frame
    protected void Update()
    {
        //if our missile has no target
        if (isHoming && !target)
        {
            currentTimer -= Time.deltaTime;

            if (currentTimer <= 0)
            {
                isHoming = false;
                currentTimer = 3f;
            }
        }

        if (!isHoming)
        {
            currentTimer -= Time.deltaTime;

            //Start Flashing
            if (!isFlashing)
            {
                StartCoroutine(StartFlashing());
            }

            if (currentTimer <= 0)
            {
                SelfDestruct();
            }
        }

        if (currentSpeed < maxSpeed)
        {
            currentSpeed += increaseSpeedRate * Time.deltaTime;
        }
    }

    protected void FixedUpdate()
    {
        if (target != null && isHoming)
        {
            Vector2 directionToTarget = (Vector2)transform.position - (Vector2)target.transform.position;
            directionToTarget.Normalize();

            float value = Vector3.Cross(directionToTarget, transform.up).z;

            if (value > 0)
            {
                rb.angularVelocity = rotateSpeed;
            }
            else if (value < 0)
            {
                rb.angularVelocity = -rotateSpeed;
            }
        }

        rb.velocity = transform.up * currentSpeed;
    }

    protected IEnumerator StartFlashing()
    {
        isFlashing = true;

        for (int i = 0; i < 20; i++)
        {
            spriteRenderer.material = matYellow;

            yield return new WaitForSeconds(0.1f);

            spriteRenderer.material = matDefault;

            yield return new WaitForSeconds(0.1f);
        }
    }

    protected void SelfDestruct()
    {
        bullet.Die();
    }

    public void LostTrackOfTarget()
    {
        isHoming = false;
    }
}
