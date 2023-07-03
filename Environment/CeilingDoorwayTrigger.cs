using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingDoorwayTrigger : MonoBehaviour
{
    public string id;
    //This is the parent object for the ceiling triggers.  They reference this to turn the 'isPlayerInside' bool on and off
    [SerializeField]
    internal bool isPlayerInside = false;
    
    //Both of these functions are just called by the doorway's child triggers
    internal void BuildingExit()
    {
        GameEvents.current.CeilingTriggerExit(id);
    }

    internal void BuildingEnter()
    {
        GameEvents.current.CeilingTriggerEnter(id);
    }
}
