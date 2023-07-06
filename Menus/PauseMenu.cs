using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;  //enable this when paused
    public GameObject gameUI;   //disable this when paused

    public GameObject controlsUI;
    public GameObject quitUI;

    public string soundEffect_ButtonMouseOver, soundEffect_ButtonPress, soundEffect_Quit, soundEffect_Pause, soundEffect_Unpause;

    public string mainMenuScene = "Scene_MainMenu";

    bool mainPageActive;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause();
        }
    }

    void SetPause()
    {
        if (GameController.gameIsActive)   //Game is not active only during summary or game over menus
        {
            if (!gameIsPaused)
            {
                Paused();
            }
            else
            {
                if (!mainPageActive)
                {
                    return;
                }

                Resume();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1;
        AudioListener.pause = false;
        gameIsPaused = false;
        GameEvents.current.GamePauseMenu(false);
        mainPageActive = false;
        PlaySound(soundEffect_Unpause);
    }
    //Call the Pause Game Event
    void Paused()
    {
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0;
        AudioListener.pause = true;
        gameIsPaused = true;
        GameEvents.current.GamePauseMenu(true);
        mainPageActive = true;
        PlaySound(soundEffect_Pause);
    }

    public void ReturnToMenu()
    {
        pauseMenuUI.SetActive(false);
        GameController.gameIsActive = true;
        Time.timeScale = 1;
        Debug.Log("Returning to Menu");
        gameIsPaused = false;
        GameEvents.current.GamePauseMenu(false);
        AudioListener.pause = false;    //Change this to a gameevent, and certain sounds (menu sounds) ignore the pausing
        SceneManager.LoadScene(mainMenuScene);
        PlaySound(soundEffect_Quit);
    }

    //Buttons
    public void MainPage(bool _bool)
    {
        pauseMenuUI.SetActive(_bool);
    }

    public void OptionsPage(bool _bool)
    {
        controlsUI.SetActive(_bool);
    }

    public void QuitPage(bool _bool)
    {
        quitUI.SetActive(_bool);
    }

    public void MainPageActive(bool _bool)
    {
        mainPageActive = _bool;
    }
    //Button Events
    public void ButtonMouseOver()
    {
        PlaySound(soundEffect_ButtonMouseOver);
    }
    public void ButtonPress()
    {
        PlaySound(soundEffect_ButtonPress);
    }
    public void PlaySound(string _soundEffectName)
    {
        GameEvents.current.PlaySound(_soundEffectName);
    }
}
