using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Material matWhite;
    Material matDefault;

    float lerpSpeed = 2f;
    float lerp = 0f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;
    }
    
    // Update is called once per frame
    void Update()
    {
        lerp += lerpSpeed * Time.deltaTime;

        spriteRenderer.material.Lerp(matDefault, matWhite, lerp);
    }
}
