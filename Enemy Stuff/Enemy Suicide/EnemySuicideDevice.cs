using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySuicideDevice : MonoBehaviour
{
    public float suicideDistance;
    public float timeToSuicideExplode;
    bool commitSuicide = false;

    public GameObject suicideExplosionEffect;
    [SerializeField] GameObject animatedChildObject;
    AnimatedObject animatedObject;

    EnemyBehavior enemyBehavior;
    EnemyRadar enemyRadar;

    // Start is called before the first frame update
    void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        enemyRadar = GetComponent<EnemyRadar>();
        animatedObject = animatedChildObject.GetComponent<AnimatedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyBehavior.currentPlayerTarget != null)
        {
            float distance = Vector2.Distance(transform.position, enemyBehavior.currentPlayerTarget.transform.position);

            if (distance < suicideDistance && enemyRadar.hasLineOfSight)
            {
                if (!commitSuicide)
                {
                    commitSuicide = true;

                    StartCoroutine(CommitSuicide());
                }
            }
        }
    }

    IEnumerator CommitSuicide()
    {
        if (animatedObject != null)
        {
            animatedObject.PlayAnimation();
        }

        yield return new WaitForSeconds(timeToSuicideExplode);

        if (suicideExplosionEffect != null)
        {
            Instantiate(suicideExplosionEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
