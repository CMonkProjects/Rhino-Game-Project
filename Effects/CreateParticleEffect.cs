using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticleEffect : MonoBehaviour
{
    public GameObject particleEffect;
    // Start is called before the first frame update
    void Start()
    {
        if (particleEffect != null)
        {
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
