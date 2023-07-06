using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This tells the chosen particle emitter to continue existing even after this object is destroyed
public class ParticleDestroyHandler : MonoBehaviour
{
    public GameObject particleEmitter;

    void OnDestroy()
    {
        var ps = particleEmitter.GetComponent<ParticleSystem>();

        particleEmitter.transform.parent = null;
        ps.Stop();
    }
}
