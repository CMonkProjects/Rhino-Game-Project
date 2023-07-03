using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFadeOut : MonoBehaviour
{
    public float maxFadeoutTime;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = maxFadeoutTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
