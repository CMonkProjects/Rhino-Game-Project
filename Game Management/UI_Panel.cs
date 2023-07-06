using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Panel : MonoBehaviour
{
    GameController gameController;
    public GameObject ui_Panel;
    //UI Text
    public Text livesText;
    public Text sceneText;
    public Text messageText;
    public List<string> activeMessages = new List<string>();

    [Header("Event messages")]
    [SerializeField] string playerSpawned;
    [SerializeField] string playerDestroyed, playerLowHealth, enemyRhinoApproaching, enemyRhinoDestroyed, enemySuicideApproaching, newObjective;

    [Header("Event Sounds")]
    [SerializeField] string eventPositive;
    [SerializeField] string eventGeneric, eventWarning, eventPlayerHurt;

    [Header("Powerup UI")]
    public Text activePowerupText;
    public List<GameObject> activePowerups;

    Scene activeScene;
    string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
        SubscribeToEvents();
        StartNewGame();
        GetSceneName();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onPlayerDied += PlayerDied;
        GameEvents.current.onPlayerSpawned += PlayerRespawned;
        GameEvents.current.onPlayerDamaged += PlayerTakenDamage;
        GameEvents.current.onMessage += NewMessage;
        GameEvents.current.onPlayerLowHealth += PlayerLowHealth;
        GameEvents.current.onEnemyRhinoNear += EnemyRhinoNear;
        GameEvents.current.onEnemySuicideNear += EnemySuicideNear;
        GameEvents.current.onStartNewGame += StartNewGame;
        //GameEvents.current.onLevelEnd += EndLevel;
        GameEvents.current.onObjectiveCompleted += ObjectiveCompleted;
        GameEvents.current.onNewObjective += NewObjective;
    }
    private void OnDestroy()
    {
        GameEvents.current.onPlayerDied -= PlayerDied;
        GameEvents.current.onPlayerSpawned -= PlayerRespawned;
        GameEvents.current.onPlayerDamaged -= PlayerTakenDamage;
        GameEvents.current.onMessage -= NewMessage;
        GameEvents.current.onPlayerLowHealth -= PlayerLowHealth;
        GameEvents.current.onEnemyRhinoNear -= EnemyRhinoNear;
        GameEvents.current.onEnemySuicideNear -= EnemySuicideNear;
        GameEvents.current.onStartNewGame -= StartNewGame;
        //GameEvents.current.onLevelEnd -= EndLevel;
        GameEvents.current.onObjectiveCompleted -= ObjectiveCompleted;
        GameEvents.current.onNewObjective += NewObjective;
    }

    //Level Loading and Ending---------------------------
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Called when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetSceneName();
        ui_Panel.SetActive(true);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //UI Stuff: Outdated but leaving it here----------------------------------------
    public void UpdateUI(string _UIToUpdate)
    {
        //score, lives, sceneName, message
        switch (_UIToUpdate)
        {
            case "Lives":
                break;

            case "SceneName":
                sceneText.text = sceneName;
                break;
        }
    }
    //Methods subscribed to events---------------------------
    void StartNewGame()
    {
        UpdateUI("Lives");
    }

    internal void UpdateLives(int _lives)
    {
        UpdateUI("Lives");
    }

    public void PowerupCollected(GameObject powerUp)
    {
        PowerUp powerUpScript = powerUp.gameObject.GetComponent<PowerUp>();

        if (!powerUpScript.expiresImmediately)
        {
            activePowerups.Add(powerUpScript.gameObject);
            UpdateActivePowerupUI();
        }
    }

    public void PowerupExpired(GameObject powerUp)
    {
        activePowerups.Remove(powerUp);
        UpdateActivePowerupUI();
    }

    public void ClearActivePowerups()
    {
        activePowerups.Clear();
        UpdateActivePowerupUI();
    }

    void UpdateActivePowerupUI()
    {
        activePowerupText.text = "";

        if (activePowerups == null || activePowerups.Count == 0)
        {
            return;
        }

        foreach(GameObject powerUp in activePowerups)
        {
            PowerUp powerUpScript = powerUp.GetComponent<PowerUp>();

            activePowerupText.text += "\n" + powerUpScript.powerupName;
        }
    }

    void NewMessage(string _newMessage)
    {
        activeMessages.Add(_newMessage);
        UpdateMessagesUI();
    }

    void UpdateMessagesUI()
    {
        messageText.text = "";

        if (activeMessages == null || activeMessages.Count == 0)
        {
            return;
        }

        //Remove active messages after a short amount of time
        foreach(string message in activeMessages)
        {
            messageText.text += "\n" + message;
            StartCoroutine(RemoveMessage(message));
        }
    }

    void PlayerDied()
    {
        UpdateLives(-1);
        NewMessage(playerDestroyed);
        ClearActivePowerups();
        GameEvents.current.PlaySound(eventWarning);
    }

    void PlayerRespawned()
    {
        NewMessage(playerSpawned);
        GameEvents.current.PlaySound(eventGeneric);
    }

    void PlayerLowHealth()
    {
        NewMessage(playerLowHealth);
        GameEvents.current.PlaySoundMultipleTimes(eventWarning, 3);
    }

    void PlayerTakenDamage(float _damage)
    {
        int playerDamage = Mathf.FloorToInt(_damage);
        GameEvents.current.PlaySoundMultipleTimes(eventPlayerHurt, playerDamage);
    }

    void EnemyRhinoNear()
    {
        NewMessage(enemyRhinoApproaching);
        GameEvents.current.PlaySoundMultipleTimes(eventWarning, 3);
    }

    void EnemySuicideNear()
    {
        NewMessage(enemySuicideApproaching);
        GameEvents.current.PlaySoundMultipleTimes(eventWarning, 2);
    }

    void EndLevel()
    {
        ui_Panel.SetActive(false);
    }

    void ObjectiveCompleted(string _objCompletedMessage)
    {
        GameEvents.current.NewMessage(_objCompletedMessage);
        GameEvents.current.PlaySound(eventPositive);
    }

    void NewObjective()
    {
        GameEvents.current.NewMessage(newObjective);
        GameEvents.current.PlaySound(eventGeneric);
    }
    //Display Messages----------------------------------
    IEnumerator RemoveMessage(string _messageToRemove)
    {
        yield return new WaitForSeconds(3f);

        activeMessages.Remove(_messageToRemove);
        UpdateMessagesUI();
    }
    //Other Stuff---------------------------------------
    void GetSceneName()
    {
        activeScene = SceneManager.GetActiveScene();
        sceneName = activeScene.name;
        UpdateUI("SceneName");
    }
}