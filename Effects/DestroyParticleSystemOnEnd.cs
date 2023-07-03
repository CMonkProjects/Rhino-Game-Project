using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Destroy the particle system after there's no life particles anymore
public class DestroyParticleSystemOnEnd : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
