using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Derived from powerup script found on raywenderlich.com
public class PowerUp : MonoBehaviour
{
    [Tooltip("Displayed in the UI")]
    public string powerupName;
    [Tooltip("Displayed upon pickup")]
    public string powerupMessage;
    [Tooltip("Check true for instantaneous powerups, such as health or score pickups, otherwise they run on a timer")]
    public bool expiresImmediately;
    public GameObject specialEffect;
    public string soundEffect;
    [Tooltip("Checks specific things before the powerup can be picked up (is not at full health, doesn't have an active shield, etc.)")]
    public bool needsConditions;
    public bool conditionResult;

    protected PlayerStats playerStats;
    protected PlayerWeaponAmmo playerWeaponAmmo;
    protected UI_Panel uiPanel;

    protected SpriteRenderer spriteRenderer;
    
    PowerUp powerUp;

    public UnityEvent onPowerupCollectedEvent;

    //Delegates
    public delegate void OnPowerupCollected();
    public OnPowerupCollected onPowerupCollected;

    protected enum PowerupState
    {
        Idle,
        Collected,
        Expiring
    }

    protected PowerupState powerupState;

    protected virtual void Awake()
    {
        //Reference the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiPanel = GameObject.FindGameObjectWithTag("GameController").GetComponent<UI_Panel>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Set the powerup default state
        powerupState = PowerupState.Idle;
        powerUp = this;

        if (onPowerupCollectedEvent == null)
            onPowerupCollectedEvent = new UnityEvent();
    }

    //When colliding with the player trigger
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PowerupCollected(collision.gameObject);
    }

    //Powerup collected function
    protected virtual void PowerupCollected(GameObject gameObjectCollectingPowerup)
    {
        if (gameObjectCollectingPowerup.tag != "Player")
        {
            return;
        }

        //Get components of player that picked us up
        playerStats = gameObjectCollectingPowerup.GetComponent<PlayerStats>();
        playerWeaponAmmo = gameObjectCollectingPowerup.transform.GetChild(0).GetComponent<PlayerWeaponAmmo>();

        if (playerStats != null && playerWeaponAmmo != null)
        {
            CheckConditions();
        }
        
        if (conditionResult == false)
        {
            Debug.Log(gameObject.name + " condition check failed, returning");
            return;
        }

        //Make sure we haven't been picked up before
        if (powerupState != PowerupState.Idle)
        {
            return;
        }
        powerupState = PowerupState.Collected;

        //Parent and position the powerup under the player (not necessary, it's just for organizational purposes)
        if (playerStats != null)
        {
            gameObject.transform.parent = playerStats.gameObject.transform;
            gameObject.transform.position = playerStats.gameObject.transform.position;
        }

        //Special effects
        PlaySpecialEffects();

        //Events
        GameEvents.current.NewMessage(powerupMessage);

        //Call the Unity Event
        onPowerupCollectedEvent.Invoke();

        //call delegate
        if (onPowerupCollected != null)
        {
            onPowerupCollected();
        }

        //The Powerup function itself
        PowerupPayload();

        uiPanel.PowerupCollected(gameObject);

        //Make the powerup sprite invisible as it's been picked up
        spriteRenderer.enabled = false;
    }

    //This is for certain powerups to check for things like if the player is not already at full health or has an active powerup already running (such as a shield or overcharge)
    protected virtual void CheckConditions()
    {
        //if statement that checks specific condition to powerup, then sets conditionResult to true if met
        if (!needsConditions)
        {
            Debug.Log(gameObject.name + " doesn't need special conditions");
            conditionResult = true;
        }
    }

    //Powerup particle and sound effects
    protected virtual void PlaySpecialEffects()
    {
        if (specialEffect != null)
        {
            //Create special effect at position, rotation and set its parent
            Instantiate(specialEffect, transform.position, transform.rotation, transform);
        }

        if (soundEffect != null)
        {
            GameEvents.current.PlaySound(soundEffect);
        }
    }

    //Powerup payload function
    protected virtual void PowerupPayload()
    {
        //For instant powerups like health, extra lives, etc.
        if (expiresImmediately)
        {
            PowerupHasExpired();
        }
    }

    //Powerup has expired function
    protected virtual void PowerupHasExpired()
    {
        if (powerupState == PowerupState.Expiring)
        {
            return;
        }
        powerupState = PowerupState.Expiring;

        //Powerup expired event (to remove any icons and text in the UI)
        uiPanel.PowerupExpired(gameObject);

        DestroySelfAfterDelay();
    }

    //Destroy self after delay
    protected virtual void DestroySelfAfterDelay()
    {
        Destroy(gameObject, 10f);
    }
}
