using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consequence_Aircraft : Consequence_Basic
{
    [SerializeField] protected bool startTimerOnTakeOff;
    bool startedTakeOff = false;
    [SerializeField] protected float takeoffTimer;

    [SerializeField] string takeoffSound;

    [SerializeField] protected float launchForce = 1;

    [SerializeField] protected GameObject particleEmitter;

    [SerializeField] protected EnemyHealth enemyHealth;

    [SerializeField] string objectiveTargetID;  //just input this manually

    protected override void OnEnable()
    {
        if (startTimerOnActive)
        {
            StartCountdown();
        }

        //delegate
        enemyHealth.onDead += UnparentParticleEmitter;
    }

    protected virtual void OnDisable()
    {
        //delegate
        enemyHealth.onDead -= UnparentParticleEmitter;
    }

    protected override void Update()
    {
        base.Update();

        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (startTimerOnTakeOff)
            {
                if (takeoffTimer > 0f)
                {
                    takeoffTimer -= Time.deltaTime;
                }
                else
                {
                    if (!startedTakeOff)
                    {
                        TakeOff();
                    }
                }
            }
        }
    }

    protected override void NegativeEvent()
    {
        firedNegativeEvent = true;

        //aircraft not destroyed event
        GameEvents.current.AircraftNotDestroyed();

        GameEvents.current.TargetNotDestroyed(objectiveTargetID);

        //unparent particle emitter
        UnparentParticleEmitter();

        Destroy(gameObject);
    }

    void TakeOff()
    {
        startedTakeOff = true;

        Rigidbody2D projectileRB = GetComponent<Rigidbody2D>();

        if (projectileRB)
        {
            projectileRB.AddForce(transform.up * launchForce, ForceMode2D.Impulse);
        }

        GameEvents.current.PlaySound(takeoffSound);

        StartCountdown();   //begin the negative event timer
    }

    protected virtual void UnparentParticleEmitter()
    {
        if (particleEmitter != null)
        {
            particleEmitter.transform.parent = null;
            particleEmitter.GetComponent<ParticleSystem>().Stop();
        }
    }
}
