using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionElevator : MonoBehaviour
{
    GameObject playerObject;
    Transform exitTrigger; //Ceiling exit trigger (set its position at the desination's transform)

    public bool isUnderground;

    public Transform destination;

    void Awake()
    {
        if (!isUnderground)
        {
            exitTrigger = transform.parent.Find("Ceiling_Trigger_Exit").gameObject.transform;   //get sibling object

            if (exitTrigger != null && destination != null)
            {
                Debug.Log("Exit Trigger found, moving its position");
                exitTrigger.transform.position = destination.transform.position;
            }
        }
    }
    public void TransportPlayer()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null && destination != null)
        {
            Debug.Log("Transporting player to new location");
            playerObject.transform.position = destination.transform.position;
            GameEvents.current.PlaySound("Elevator");
        }
    }
}
