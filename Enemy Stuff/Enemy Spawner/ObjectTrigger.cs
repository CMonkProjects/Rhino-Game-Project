using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] bool isTriggered;
    [SerializeField] List<GameObject> objectsTracked;

    float timer = 0f;
    float triggerTimer = 0.5f;

    public UnityEvent onObjectsDestroyed;

    [SerializeField] string destroyedTriggerMessage;

    void Update()
    {
        if (!isTriggered)
        {
            timer += Time.deltaTime;

            if (timer >= triggerTimer)
            {
                CheckForEnemies();
                timer = 0f;
            }
        }
    }

    void CheckForEnemies()
    {
        //When objects in a list are destroyed they're "missing" (or null), so we use a linq and remove any null items in the list
        objectsTracked.RemoveAll(item => item == null);

        //If our list of tracked objects is empty then the bool is true
        bool allObjectsAreDead = objectsTracked.Count == 0;

        if (allObjectsAreDead)
        {
            GameEvents.current.NewMessage(destroyedTriggerMessage);
            onObjectsDestroyed.Invoke();
            isTriggered = true;
        }
    }
}
