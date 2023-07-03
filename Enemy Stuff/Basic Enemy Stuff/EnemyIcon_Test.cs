using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIcon_Test : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(target.position + offset);

            if (transform.position != pos)
                transform.position = pos;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
