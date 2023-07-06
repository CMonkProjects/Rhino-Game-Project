using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjective : MonoBehaviour
{
    [SerializeField] internal string objectiveTargetID;

    public void AssignTargetID(string _objTarget)
    {
        objectiveTargetID = _objTarget;
    }

    void OnDestroy()
    {
        GameEvents.current.TargetObjectiveDestroyed(objectiveTargetID);
    }
}
