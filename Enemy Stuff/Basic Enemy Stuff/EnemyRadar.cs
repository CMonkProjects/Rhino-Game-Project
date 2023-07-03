using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class EnemyRadar : MonoBehaviour
{
    [Header("This is the enemy's way of detecting the player")]
    [Header("If player is in radar range then enemy sees player")]

    [SerializeField] LayerMask layerMask;

    [SerializeField] Transform currentTarget = null;
    GameController gameController;
    protected internal bool hasLineOfSight;

    [SerializeField] float radarRadius;
    float radarScanTime = 0.5f;    //searches for player every half second
    float radarTimer = 0f;

    [SerializeField] bool canDetectPlayer = true;
    [SerializeField] internal bool enemyAlertChasing = false;   //enemy knows where player is at all times and never stops chasing
    [SerializeField] LayerMask obstacleMask;
    public EnemyHealth enemyHealth;

    EnemyBehavior enemyBehavior;

    // Start is called before the first frame update
    void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        enemyHealth = GetComponent<EnemyHealth>();
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (canDetectPlayer)
            {
                radarTimer += Time.deltaTime;

                if (radarTimer >= radarScanTime)
                {
                    radarTimer = 0f;

                    switch (enemyAlertChasing)
                    {
                        case true:
                            EnemyAlertScan();   //alert
                            break;

                        case false:
                            PassiveScan();  //passive
                            break;
                    }
                }
            }
        }

        if (canDetectPlayer)
        {
            if (currentTarget)
            {
                Debug.DrawLine(transform.position, currentTarget.position, Color.red);
            }
        }
    }

    public void CheckLineOfSight()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radarRadius, layerMask);

        if (hit)
        {
            if (hit.tag == "Player")
            {
                if (!Physics2D.Linecast(transform.position, hit.transform.position, obstacleMask))
                {
                    currentTarget = hit.transform;
                    hasLineOfSight = true;
                }
            }
            else hasLineOfSight = false;
        }
    }

    public void PassiveScan()
    {
        CheckLineOfSight();

        if (hasLineOfSight)
        {
            EnemyAlerted();
        }

        //Set the target for the enemy AI script
        if (enemyBehavior != null)
        {
            enemyBehavior.PlayerDetected(currentTarget);
        }
    }

    public void EnemyAlerted()
    {
        Debug.LogWarning(gameObject.name + " enemy alerted");
        enemyAlertChasing = true;
    }

    public void EnemyAlertScan()
    {
        try
        {
            if (currentTarget == null)
            {
                if (gameController)
                {
                    currentTarget = gameController.playerTransform;
                }
            }
        }
        catch(NullReferenceException e)
        {
            //This prevents showing an error in console if currentTarget is null
        }

        if (currentTarget != null)
        {
            enemyAlertChasing = true;
        }

        CheckLineOfSight();

        //Set the target for the enemy AI script
        if (enemyBehavior != null)
        {
            enemyBehavior.PlayerDetected(currentTarget);
        }
    }
    /*//Draw radar detection radius
    void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, radarRadius);
    }*/
}
