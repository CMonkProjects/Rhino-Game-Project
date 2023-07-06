using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooled : Bullet
{
    [SerializeField] string pooledObjectTag;
    void OnEnable()
    {
        GameEvents.current.PlaySound(firingSound);
    }

    protected override void Start()
    {
        //do nothing
    }

    public override void Die()
    {
        if (hitEffect != null)
        {
            GameObject impactEffect = ObjectPooler.sharedInstance.GetPooledObject(pooledObjectTag);

            if (impactEffect != null)
            {
                impactEffect.transform.position = transform.position;
                impactEffect.transform.rotation = transform.rotation;
                impactEffect.SetActive(true);
            }
        }

        if (particleEmitter != null)
        {
            particleEmitter.transform.parent = null;
            particleEmitter.GetComponent<ParticleSystem>().Stop();
            Destroy(particleEmitter, 1f);
        }

        gameObject.SetActive(false);
    }
}
