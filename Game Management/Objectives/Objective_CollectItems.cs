using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Objective_CollectItems : Objective
{
    public string objectiveItemType;
    int currentItems;
    public int objectiveItemTotal;

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

    void UnityEventTriggered()
    {
        completedUnityEvent.Invoke();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onCollectObjectiveItem += ObjectiveItemCollected;
    }

    private void OnDestroy()
    {
        GameEvents.current.onCollectObjectiveItem -= ObjectiveItemCollected;
    }

    public override bool CheckConditions()
    {
        return (currentItems >= objectiveItemTotal);
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
            objectiveText.text = string.Format(objectiveDescription + ": {0} / {1}", currentItems, objectiveItemTotal);

            GameEvents.current.ObjectiveUpdated();
        }
    }

    void ObjectiveItemCollected(string _objItemType)
    {
        if (_objItemType == objectiveItemType)
        {
            currentItems++;
        }

        if (CheckConditions())
        {
            Completed();
        }

        UpdateHUD();
    }
}
