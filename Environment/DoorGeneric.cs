using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGeneric : MonoBehaviour
{
    public void DoorOpen()
    {
        gameObject.SetActive(false);
        GameEvents.current.PlaySound("Door");
    }

    public void DoorClose()
    {
        gameObject.SetActive(true);
        GameEvents.current.PlaySound("Door");
    }
}
