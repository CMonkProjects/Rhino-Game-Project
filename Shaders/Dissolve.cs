using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sample dissolve material script taken from Brackey's Shader Tutorial
public class Dissolve : MonoBehaviour
{
    Material material;

    bool isDissolving = false;
    float fade = 1f;

    string fadeReference = "_Fade";

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDissolving = true;
        }

        if (isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;
            }
        }

        //Set our material's "_Fade" value to fade (so it decreases)
        material.SetFloat(fadeReference, fade);
    }
}
