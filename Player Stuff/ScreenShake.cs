using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Animator cameraAnim;

    string randomIntName = "RandomInt";
    
    void Start()
    {
        GameEvents.current.onShakeScreen += CamShake;
    }

    void OnDisable()
    {
        GameEvents.current.onShakeScreen -= CamShake;
    }

    public void CamShake(string _shakeType)
    {
        int randomInt = Random.Range(1, 3);
        cameraAnim.SetTrigger(_shakeType);
        cameraAnim.SetInteger(randomIntName, randomInt);
    }
}
