using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTextActivator : MonoBehaviour
{
    public ScrollingText textEffect;

    void Awake()
    {
        textEffect = GetComponent<ScrollingText>();
    }

    /*void OnEnable()
    {
        if (textEffect)
        {
            textEffect.StartPrintingText();
        }
    }*/
}
