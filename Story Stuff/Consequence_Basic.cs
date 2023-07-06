using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Consequence_Basic : MonoBehaviour
{
    [SerializeField] protected bool startTimerOnActive;
    [SerializeField] protected bool startTimerWhenPlayerNear;

    [SerializeField] protected float countdownTimer;
    protected float timer;
    protected bool countdownActive = false;
    protected bool firedNegativeEvent = false;

    protected float radarScanTime = 0.5f;
    protected float scanTimer = 0f;
    [SerializeField] float radarRadius;

    [SerializeField] protected string playerTag = "Player";
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected LayerMask obstacleMask;

    protected virtual void OnEnable()
    {
        if (startTimerOnActive)
        {
            StartCountdown();
        }
    }

    protected void StartCountdown()
    {
        timer = countdownTimer;
        countdownActive = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (countdownActive)
            {
                if (timer > 0f)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    if (!firedNegativeEvent)
                    {
                        NegativeEvent();
                    }
                }
            }

            if (startTimerWhenPlayerNear && !countdownActive)
            {
                scanTimer += Time.deltaTime;

                if (scanTimer >= radarScanTime)
                {
                    scanTimer = 0f;

                    if (CheckForPlayerNear())
                    {
                        StartCountdown();
                    }
                }
            }
        }
    }

    protected virtual void NegativeEvent()
    {
        firedNegativeEvent = true;

        //negative events go here
    }

    protected virtual bool CheckForPlayerNear()
    {
        if (CheckIfPlayerInLineOfSight())
        {
            return true;
        }

        return false;
    }

    protected virtual bool CheckIfPlayerInLineOfSight()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radarRadius, layerMask);

        if (hit)
        {
            if (hit.tag == playerTag)
            {
                if (!Physics2D.Linecast(transform.position, hit.transform.position, obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    //Debug radar radius and other stuff
    /*void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, radarRadius);
    }*/
}
