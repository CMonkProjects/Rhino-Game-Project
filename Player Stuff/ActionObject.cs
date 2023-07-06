using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This script is for 'action objects': things such as switches that do something when the player presses the action button
public class ActionObject : MonoBehaviour
{
    public UnityEvent myUnityEvent;

    void Awake()
    {
        if (myUnityEvent == null)
            myUnityEvent = new UnityEvent();
    }

    public void DoAction()
    {
        myUnityEvent.Invoke();
    }
}