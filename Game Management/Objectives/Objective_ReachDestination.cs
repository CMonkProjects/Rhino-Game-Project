using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Objective_ReachDestination : Objective
{
    public string destinationID;
    bool destinationReached = false;

    public string objectiveCompletedMessage;

    public Text objectiveText;
    public string objectiveDescription;

    public UnityEvent completedUnityEvent;
    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
        UpdateHUD();

        if (completedUnityEvent == null)
            completedUnityEvent = new UnityEvent();
    }

    void OnEnable()
    {
        GameEvents.current.NewObjective();
    }

    void UnityEventTriggered()
    {
        completedUnityEvent.Invoke();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onDestinationReached += DestinationReached;
    }

    void OnDestroy()
    {
        GameEvents.current.onDestinationReached += DestinationReached;
    }

    public override bool CheckConditions()
    {
        return destinationReached;
    }

    public override void Completed()
    {
        objectiveCompleted = true;
        GameEvents.current.ObjectiveCompleted(objectiveCompletedMessage);

        if (objectiveText != null)
        {
            objectiveText.color = Color.green;
        }

        UnityEventTriggered();
    }

    public override void UpdateHUD()
    {
        if (objectiveText != null)
        {
            objectiveText.text = string.Format(objectiveDescription);

            GameEvents.current.ObjectiveUpdated();
        }
    }
    
    void DestinationReached(string _objDestinationID)
    {
        if (_objDestinationID == destinationID && !destinationReached)
        {
            destinationReached = true;
        }

        if (CheckConditions())
        {
            Completed();
        }

        UpdateHUD();
    }
}
