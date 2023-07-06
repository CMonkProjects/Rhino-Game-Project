using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamageMegacrush : AOEDamage
{
    [SerializeField] float timeBetweenDamagingEnemies;

    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask obstacleMask;

    [SerializeField] protected float tracerLength = 1f;

    public GameObject impactEffect;
    public LineRenderer lineRenderer;
    public float lineStartWidth = 0.03f;
    public float lineEndWidth = 0.03f;

    float currentTimer;
    [SerializeField] float damageTimer;
    void Awake()
    {
        LineRenderer line = Instantiate(lineRenderer);
        lineRenderer = line;

        lineRenderer.startWidth = lineStartWidth;
        lineRenderer.endWidth = lineEndWidth;

        lineRenderer.enabled = false;
    }
    void Start()
    {
        currentTimer = damageTimer;
        StartCoroutine(ZapEnemies());
    }

    void Update()
    {
        currentTimer -= Time.deltaTime;

        if (currentTimer < 0f)
        {
            Die();
        }
    }

    IEnumerator ZapEnemies()
    {
        while (currentTimer >= 0f)
        {
            //Check all enemies within the overlap circle and apply damage (that aren't behind any walls), and we draw lines pointing to each one
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

            foreach (Collider2D enemy in colliders)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                if (enemy.tag == "Enemy")
                {

                    Vector3 dirToTarget = (enemy.transform.position - transform.position).normalized;
                    float distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);

                    RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, distanceToTarget, enemyMask);
                    RaycastHit2D hitNoWalls = Physics2D.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask);

                    if (hit && !hitNoWalls)
                    {
                        
                        if (enemyHealth != null)
                        {
                            //enemyHealth.TakeDamage(damage);
                        }

                        //Create the impact effect at the place we hit
                        //Quaternion Identity is just a fancy way of saying 'no rotation'
                        Instantiate(impactEffect, hit.point, Quaternion.identity);

                        //draw the line representing the laser
                        lineRenderer.SetPosition(0, transform.position);
                        lineRenderer.SetPosition(1, hit.point);

                        //draw the line renderer representing the laser
                        lineRenderer.enabled = true;
                        lineRenderer.sortingOrder = 0;

                        yield return new WaitForSeconds(timeBetweenDamagingEnemies);

                        lineRenderer.enabled = false;
                    }
                }
            }

            yield return null;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (lineRenderer != null)
            Destroy(lineRenderer);
    }
}
