using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectWreckageBuilding : MonoBehaviour
{
    //create a random number of particle prefabs (smoke and fire)
    [SerializeField] GameObject smokeParticleEffect, fireParticleEffect;

    [SerializeField] int maxSmokeEffects, maxFireEffects;

    //spawn the random particle prefabs randomly around a square area
    [SerializeField] float minPos, maxPos;

    //small, 16 x 16 pixel building

    //-0.08, 0.08 x y

    //large, 32 x 32 pixel building

    //-0.16, 0.16 x y


    // Start is called before the first frame update
    void Start()
    {
        SpawnWreckageEffects();

        Destroy(gameObject);
    }

    void SpawnWreckageEffects()
    {
        float smokeEffectsToSpawn = Random.Range(1, maxSmokeEffects + 1);
        float fireEffectsToSpawn = Random.Range(1, maxFireEffects + 1);

        for (int i = 0; i < smokeEffectsToSpawn; i++)
        {
            var randomPosition = new Vector3(Random.Range(minPos, maxPos), Random.Range(minPos, maxPos));

            Instantiate(smokeParticleEffect, transform.position + randomPosition, Quaternion.identity);
        }

        for (int i = 0; i < fireEffectsToSpawn; i++)
        {
            var randomPosition = new Vector3(Random.Range(minPos, maxPos), Random.Range(minPos, maxPos));

            Instantiate(fireParticleEffect, transform.position + randomPosition, Quaternion.identity);
        }
    }
}
