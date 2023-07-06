using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Objective_Destroy : Objective
{
    public List<GameObject> targetList;
    public string objectiveTargetID;
    int targetCountTotal;
    [SerializeField] int failPoints = 0;

    public string objectiveCompletedMessage;

    public Text objectiveText;

    public string objectiveDescription;

    public UnityEvent completedUnityEvent;

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
        DesignateTargets();
        UpdateHUD();

        if (completedUnityEvent == null)
            completedUnityEvent = new UnityEvent();
    }

    //Called when this specific objective is completed
    void UnityEventTriggered()
    {
        completedUnityEvent.Invoke();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onTargetObjectiveDestroyed += TargetDestroyed;
        GameEvents.current.onTargetNotDestroyed += TargetEscaped;
    }

    void OnDestroy()
    {
        GameEvents.current.onTargetObjectiveDestroyed -= TargetDestroyed;
        GameEvents.current.onTargetNotDestroyed -= TargetEscaped;
    }

    //Add a target objective script to all the game objects in our list and give them a target ID
    void DesignateTargets()
    {
        foreach(GameObject target in targetList)
        {
            target.AddComponent<TargetObjective>();
            target.GetComponent<TargetObjective>().AssignTargetID(objectiveTargetID);

            MinimapIcon _minimap = target.GetComponent<MinimapIcon>();

            if (_minimap != null)
            {
                _minimap.CreateTargetMinimapObject();
            }
            else Debug.LogError(_minimap.gameObject.name + " doesn't have a minimap script!");
        }

        targetCountTotal = targetList.Count;
    }
    
    //Objective is complete if these conditions are fufilled
    public override bool CheckConditions()
    {
        return (targetList.Count <= 0);
    }

    public override void Completed()
    {
        //only fire this once
        if (objectiveCompleted == false)
        {
            objectiveCompleted = true;
            GameEvents.current.ObjectiveCompleted(objectiveCompletedMessage);

            if (objectiveText != null)
            {
                if (failPoints >= targetCountTotal)
                {
                    objectiveText.color = Color.red;
                }
                else if (failPoints > 0 && failPoints < targetCountTotal)
                {
                    objectiveText.color = Color.yellow;
                }
                else
                {
                    objectiveText.color = Color.green;
                }
            }

            UnityEventTriggered();
        }
    }

    public override void UpdateHUD()
    {
        if (objectiveText != null)
        {
            objectiveText.text = string.Format(objectiveDescription + ": {0} / {1}", targetList.Count, targetCountTotal);

            GameEvents.current.ObjectiveUpdated();
        }
    }

    //Whenever a target object is destroyed this event fires
    public void TargetDestroyed(string _objTargetID)
    {
        //Check if has a target ID relevent to this objective
        if (_objTargetID != objectiveTargetID)
            return;

        StartCoroutine(UpdateHUDCoroutine());
    }

    void TargetEscaped(string _objTargetID)
    {
        //Check if has a target ID relevent to this objective
        if (_objTargetID != objectiveTargetID)
            return;

        //fail points
        failPoints++;

        StartCoroutine(UpdateHUDCoroutine());
    }

    //Called whenever we destroy a target object for this objective
    IEnumerator UpdateHUDCoroutine()
    {
        yield return new WaitForEndOfFrame();

        targetList.RemoveAll(GameObject => !GameObject);

        UpdateHUD();

        if (CheckConditions())
        {
            Completed();
        }
    }
}
