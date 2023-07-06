using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveGroupEvents : MonoBehaviour
{
    public List<ObjectiveEvent> objEvents;

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onAllObjectivesCompleted += OnObjectiveGroupComplete;
        GameEvents.current.onNewObjectiveGroup += OnNewObjective;
    }

    void OnDestroy()
    {
        GameEvents.current.onAllObjectivesCompleted -= OnObjectiveGroupComplete;
        GameEvents.current.onNewObjectiveGroup -= OnNewObjective;
    }

    void OnObjectiveGroupComplete(int _objGroup)
    {
        objEvents[_objGroup].OnObjectiveGroupCompleteEvent.Invoke();
    }

    void OnNewObjective(int _objGroup)
    {
        objEvents[_objGroup].OnNewObjectiveGroupEvent.Invoke();
    }
}

[System.Serializable]
public class ObjectiveEvent
{
    public int objGroupID;
    [Header("Called when this new objective group is enabled")]
    public UnityEvent OnNewObjectiveGroupEvent;
    [Header("Called when all objectives in this group are completed")]
    public UnityEvent OnObjectiveGroupCompleteEvent;
}
