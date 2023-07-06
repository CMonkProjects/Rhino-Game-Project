using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionButton : MonoBehaviour
{
    public ActionObject actionObject;

    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (actionObject != null)
                {
                    actionObject.DoAction();
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ActionTrigger")
        {
            actionObject = collision.GetComponent<ActionObject>();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        actionObject = null;
    }
}
