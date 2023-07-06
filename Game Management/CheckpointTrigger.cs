using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int id;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            GameEvents.current.CheckpointTrigger(id);
    }
}
