using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;  //subscribing (listening) to the onDoorwayTriggerEnter event
        GameEvents.current.onDoorwayTriggerExit += OnDoorwayClose;
    }

    void OnDoorwayOpen(int id)    //this event fires when the onDoorwayTriggerEnter event is called
    {
        if (id == this.id)
        {
            gameObject.SetActive(false);
            GameEvents.current.PlaySound("Door");
        }
    }

    void OnDoorwayClose(int id)
    {
        if (id == this.id)
        {
            gameObject.SetActive(true);
            GameEvents.current.PlaySound("Door");
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit -= OnDoorwayClose;
    }
}
