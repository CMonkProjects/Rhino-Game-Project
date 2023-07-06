using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consequence_LRM : Consequence_Basic
{
    [SerializeField] protected float launchForce = 1;

    [SerializeField] protected GameObject particleEmitter;

    [SerializeField] string firingSound;

    [SerializeField] protected EnemyHealth enemyHealth;

    [SerializeField] string objectiveTargetID;

    protected override void OnEnable()
    {
        base.OnEnable();

        enemyHealth.onDead += UnparentParticleEmitter;        //listen to the delegate
    }

    protected virtual void OnDisable()
    {
        enemyHealth.onDead -= UnparentParticleEmitter;
    }
    void Start()
    {
        Rigidbody2D projectileRB = GetComponent<Rigidbody2D>();

        if (projectileRB)
        {
            projectileRB.AddForce(transform.up * launchForce, ForceMode2D.Impulse);
        }

        GameEvents.current.PlaySound(firingSound);
    }

    protected override void NegativeEvent()
    {
        base.NegativeEvent();

        //LRM Event
        GameEvents.current.LRMNotDestroyed();

        GameEvents.current.TargetNotDestroyed(objectiveTargetID);

        GameEvents.current.NewMessage("Enemy LRM Escaped");

        UnparentParticleEmitter();

        Destroy(gameObject);
    }

    protected virtual void UnparentParticleEmitter()
    {
        if (particleEmitter != null)
        {
            particleEmitter.transform.parent = null;
            particleEmitter.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void SetTargetID(string _targetID)
    {
        objectiveTargetID = _targetID;

        Debug.LogError("LRM target ID is - " + objectiveTargetID);
    }
}
