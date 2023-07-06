using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentOnAwakeBehavior : MonoBehaviour
{
    void Awake()
    {
        transform.parent = null;
    }
}
