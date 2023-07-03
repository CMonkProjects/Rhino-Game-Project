using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingTrigger : MonoBehaviour
{
    [Tooltip("We have two triggers per doorway: an enter and an exit.  The enter trigger will set the player as inside the building, while the seperate exit trigger will set him to outside")]
    public bool isInterior;
    [SerializeField]
    CeilingDoorwayTrigger ceilingDoorwayTrigger;

    void Awake()
    {
        ceilingDoorwayTrigger = transform.parent.GetComponent<CeilingDoorwayTrigger>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && ceilingDoorwayTrigger.isPlayerInside && !isInterior)  //if player is leaving the building
        {
            //GameEvents.current.CeilingTriggerExit(id);
            ceilingDoorwayTrigger.BuildingExit();
            ceilingDoorwayTrigger.isPlayerInside = false;
            Debug.Log("Player is outside = " + ceilingDoorwayTrigger.isPlayerInside);
        }
    }

    void OnTriggerStay2D(Collider2D collision)  //if player is entering the building (note that we use OnTriggerStay)
    {
        if (collision.tag == "Player" && !ceilingDoorwayTrigger.isPlayerInside && isInterior)
        {
            //GameEvents.current.CeilingTriggerEnter(id);
            ceilingDoorwayTrigger.BuildingEnter();
            ceilingDoorwayTrigger.isPlayerInside = true;
            Debug.Log("On Trigger Stay, Player is Inside = " + ceilingDoorwayTrigger.isPlayerInside);
        }
    }
}
