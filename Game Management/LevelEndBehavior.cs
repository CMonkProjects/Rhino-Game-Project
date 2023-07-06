using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndBehavior : MonoBehaviour
{
    //Simple script that calls the Level End event if triggered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Level End Event
            GameEvents.current.LevelEnd();
        }
    }

    //Called by other objects
    public void EndLevel()
    {
        StartCoroutine(LevelEnd());
    }

    IEnumerator LevelEnd()
    {
        GameEvents.current.NewMessage("Level Complete");

        //stop coroutine while game is paused
        yield return new WaitWhile(() => PauseMenu.gameIsPaused && !GameController.gameIsActive && Time.timeScale == 0f);

        yield return new WaitForSecondsRealtime(2f);

        GameEvents.current.LevelEnd();
    }
}
