using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Objective_Generic : Objective
{
    //public bool objectiveCompleted;

    public string objectiveCompletedMessage;

    public Text objectiveText;
    public string objectiveDescription;

    public UnityEvent completedUnityEvent;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHUD();

        if (completedUnityEvent == null)
            completedUnityEvent = new UnityEvent();
    }

    void UnityEventTriggered()
    {
        completedUnityEvent.Invoke();
    }

    //checks the conditions to see if the objective is complete
    public override bool CheckConditions()
    {
        return true;
    }

    //objected completed function
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

    //a more flexible generic objective called by unity events (for misc. objectives like 'Activate door switch', 'Take key to red square', etc.)
    public void GenericObjectiveCompleted()
    {
        CheckConditions();
    }

    public override void UpdateHUD()
    {
        if (objectiveText != null)
        {
            objectiveText.text = string.Format(objectiveDescription);

            GameEvents.current.ObjectiveUpdated();
        }
    }
}
