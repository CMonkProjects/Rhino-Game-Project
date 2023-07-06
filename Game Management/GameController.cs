using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    UI_Panel uiPanel;
    MouseCursor mouseCursor;

    public static bool gameIsActive = true; //Is only false when Summary or Game Over screens are active
    public static bool playerAlive;

    public Transform playerTransform;

    [SerializeField] internal int lives = 1;

    [SerializeField] string endScene;

    // Start is called before the first frame update
    void Start()
    {
        uiPanel = GetComponent<UI_Panel>();
        mouseCursor = GetComponent<MouseCursor>();
        SubscribeToEvents();
        StartNewGame();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onPlayerDied += PlayerDied;
        GameEvents.current.onPlayerSpawned += PlayerSpawned;
        GameEvents.current.onPlayerDamaged += PlayerDamaged;
        GameEvents.current.onPauseMenu += SetMousePaused;
        GameEvents.current.onGameOver += SetMouseCursorMenu;
        GameEvents.current.onLevelEnd += SetMouseCursorMenu;
        GameEvents.current.onLevelEnd += LevelEnd;
    }

    private void OnDestroy()
    {
        GameEvents.current.onPlayerDied -= PlayerDied;
        GameEvents.current.onPlayerSpawned -= PlayerSpawned;
        GameEvents.current.onPlayerDamaged -= PlayerDamaged;
        GameEvents.current.onPauseMenu -= SetMousePaused;
        GameEvents.current.onGameOver -= SetMouseCursorMenu;
        GameEvents.current.onLevelEnd -= SetMouseCursorMenu;
        GameEvents.current.onLevelEnd -= LevelEnd;
    }
    void StartNewGame()
    {
        gameIsActive = true;
    }

    //Game Management Stuff--------------------------------
    public void UpdateLives(int _lives)
    {
        lives += _lives;
        uiPanel.UpdateLives(_lives);
    }


    public void PlayerDied()
    {
        if (!gameIsActive)
        {
            return;
        }

        UpdateLives(-1);
        //playerDeaths++;
        playerAlive = false;
        SetMouseVisibility(playerAlive);    //disable the mouse cursor

        playerTransform = null;

        //If we're not out of lives yet then respawn, else gameover
        if (lives < 0)
        {
            StartCoroutine(PlayerLoses());
        }
        else
        {
            GameEvents.current.RespawnPlayer(); //call respawn player event
        }
    }

    public void PlayerSpawned()
    {
        playerAlive = true;
        SetMouseCursorCrosshair(); 
        SetMouseVisibility(true);
        playerTransform = GameObject.Find("Player Rhino(Clone)").transform;
    }

    void PlayerDamaged(float _damage)
    {
        GameEvents.current.ShakeScreen("ShakeLarge");
    }

    IEnumerator PlayerLoses()
    {
        yield return new WaitForSeconds(2f);
        //Display Game Over Menu (player has to start level again)
        GameEvents.current.GameOver();
    }

    void LevelEnd()
    {
        StartCoroutine(EndLevel());
    }

    IEnumerator EndLevel()
    {
        GameEvents.current.NewMessage("Mission Complete");

        //stop coroutine while game is paused
        yield return new WaitWhile(() => PauseMenu.gameIsPaused && !gameIsActive && Time.timeScale == 0f);

        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene(endScene);
    }

    void SetMouseCursorCrosshair()
    {
        mouseCursor.SetPlayerCursor();
        SetMouseVisibility(true);
    }

    void SetMouseCursorMenu()
    {
        mouseCursor.SetMenuCursor();
        SetMouseVisibility(true);
    }

    void SetMouseVisibility(bool _visibility)
    {
        mouseCursor.MouseVisibility(_visibility);
    }

    void SetMousePaused(bool _isPaused)
    {
        switch (_isPaused)
        {
            case true:
                SetMouseCursorMenu();
                SetMouseVisibility(true);
                break;

            case false:
                SetMouseCursorCrosshair();
                SetMouseVisibility(playerAlive);
                break;
        }
    }
}