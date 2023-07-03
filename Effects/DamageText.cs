using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition += new Vector3(0, 0.5f, 0);
        Destroy(gameObject, 1f);
    }
}
