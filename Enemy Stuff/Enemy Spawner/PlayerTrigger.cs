using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//A trigger that fires when the player touches it, can be used for anything (enemy spawns, doors, etc.)
public class PlayerTrigger : MonoBehaviour
{
    public UnityEvent onPlayerTriggerEnter;
    public UnityEvent onPlayerTriggerExit;

    [SerializeField] string enterTriggerMessage;
    [SerializeField] string exitTriggerMessage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            onPlayerTriggerEnter.Invoke();

            Debug.LogError(enterTriggerMessage);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            onPlayerTriggerExit.Invoke();

            Debug.LogError(exitTriggerMessage);
        }
    }
}