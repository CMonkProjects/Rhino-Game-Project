using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreenEvent : MonoBehaviour
{
    public enum ShakeTrigger
    {
        Shake,
        ShakeLarge
    };

    //Select the enum we want in the inspector
    public ShakeTrigger shakeTrigger = ShakeTrigger.Shake;
    
    void OnEnable()
    {
        //Argument requires strings, so we convert the enum
        GameEvents.current.ShakeScreen(shakeTrigger.ToString());
    }
}
