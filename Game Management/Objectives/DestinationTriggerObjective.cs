using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationTriggerObjective : MonoBehaviour
{
    public string objDestinationID;
    bool playerTriggered = false;

    public string description;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !playerTriggered)
        {
            playerTriggered = true;
            GameEvents.current.DestinationReached(objDestinationID);
        }
    }
}
