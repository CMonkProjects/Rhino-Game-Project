using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public int id;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        GameEvents.current.DoorwayTriggerEnter(id);   //calls an event in the Game Event object
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            GameEvents.current.DoorwayTriggerExit(id);    //calls an event in the Game Event object
    }
}