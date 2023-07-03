using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attaches particle effect to parent object (used for dying effects)
public class ParticleEffectAttacher : MonoBehaviour
{
    Transform attachedObject;

    // Update is called once per frame
    void Update()
    {
        if (attachedObject)
        {
            transform.position = new Vector3(attachedObject.transform.position.x, attachedObject.transform.position.y);
        }
    }

    public void AttachToObject(Transform _object)
    {
        attachedObject = _object;
    }
}
