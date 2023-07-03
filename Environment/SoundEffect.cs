using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public string soundEffectName;

    public bool objectBeingPooled;

    void OnDisable()
    {
        if (objectBeingPooled)
        {
            objectBeingPooled = false;
        }
    }

    void OnEnable()
    {
        //This is to keep sound effects from playing when the pooled object is instantiated at the beginning of the scene
        if (!objectBeingPooled)
        {
            GameEvents.current.PlaySound(soundEffectName);
        }
    }
}