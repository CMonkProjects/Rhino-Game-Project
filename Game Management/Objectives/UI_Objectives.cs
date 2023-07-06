using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Objectives : MonoBehaviour
{
    [SerializeField] List<Objective> objectivesCompletedList = new List<Objective>();

    public List<GameObject> objectiveGroup = new List<GameObject>();
    [SerializeField] int currentObjectiveGroup;

    bool startCoroutineOnce = true;

    [SerializeField] Transform objectivesParent;

    float currentTimer;

    void Start()
    {
        GetComponents<Objective>();

        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onObjectiveCompleted += CheckAllObjectivesComplete;
    }

    void OnDestroy()
    {
        GameEvents.current.onObjectiveCompleted -= CheckAllObjectivesComplete;
    }

    public void GrabNewObjectives()
    {

        //Grab every objective object under their group parent object
        foreach (Transform childObj in objectiveGroup[currentObjectiveGroup].transform)
        {
            //need this and the recursion at the end to make sure we grab every child objective object, or else it will miss some
            if (null == childObj)
                continue;
            
            childObj.SetParent(gameObject.transform, true);
            objectivesCompletedList.Add(childObj.GetComponent<Objective>());
            GrabNewObjectives();    //recursion
        }

        if (objectiveGroup[currentObjectiveGroup].transform.childCount == 0 && startCoroutineOnce)
        {
            startCoroutineOnce = false;
            StartCoroutine(ActivateNewObjectives());
            GameEvents.current.NewObjectiveGroup(currentObjectiveGroup);
        }
    }

    IEnumerator ActivateNewObjectives()
    {
        //activate objective objects that are now children to our UI_Objective gameObject
        foreach (Transform childObj in transform)
        {
            childObj.gameObject.SetActive(true);
            GameEvents.current.NewObjective();
        }

        yield return new WaitForEndOfFrame();
    }

    void CheckAllObjectivesComplete(string _message)
    {
        foreach (Objective objective in objectivesCompletedList)
        {
            Objective obj = objective.GetComponent<Objective>();

            if (obj.objectiveCompleted == false)
            {
                return;
            }
        }

        StartCoroutine(AllObjectivesComplete());
    }

    IEnumerator AllObjectivesComplete()
    {
        GameEvents.current.NewMessage("All Objectives Complete");
        GameEvents.current.AllObjectivesCompleted(currentObjectiveGroup);

        yield return new WaitForEndOfFrame();

        Invoke("ClearObjectives", 2f);
    }

    void ClearObjectives()
    {
        objectivesCompletedList.Clear();

        foreach (Transform childObj in transform)
        {
            Destroy(childObj.gameObject);
        }

        if (currentObjectiveGroup < objectiveGroup.Count - 1)
        {
            currentObjectiveGroup++;

            startCoroutineOnce = true;

            GrabNewObjectives();
        }
    }
}

//base class for all objectives
public abstract class Objective : MonoBehaviour
{
    public bool objectiveCompleted;
    public abstract bool CheckConditions();  //checks the conditions to see if the objective is complete
    public abstract void Completed();   //objected completed function
    public abstract void UpdateHUD();
}