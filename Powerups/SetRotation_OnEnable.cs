using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotation_OnEnable : MonoBehaviour
{
    void OnEnable()
    {
        transform.rotation = Quaternion.identity;
    }
}
