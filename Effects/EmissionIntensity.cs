using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All the building rubble sprites have glowing embers, this oscillates the emission map's intensity
public class EmissionIntensity : MonoBehaviour
{
    public Material material;
    public SpriteRenderer spriteRenderer;

    public Color emissionColor;

    public float intensityMax = 1f;
    float intensity;
    public float intensitySpeed = 0.5f;
    void Start()
    {
        //emissionColor = material.GetColor(material.color);    //get the default emission map color
    }

    // Update is called once per frame
    void Update()
    {
        intensity = Mathf.PingPong(Time.time * intensitySpeed, intensityMax);

        //Set the intensity of the emission map
        if (material)
        {
            //material.SetColor("_Color", new Color(emissionColor.r * intensity, emissionColor.g * intensity, emissionColor.b * intensity));
            material.color = new Color(emissionColor.r * intensity, emissionColor.g * intensity, emissionColor.b * intensity);
        }
    }
}
