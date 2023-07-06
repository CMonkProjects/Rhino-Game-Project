using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Derived from BlackThornProd health UI tutorial
public class UI_Health_Old : MonoBehaviour
{
    PlayerStats playerHealth;

    [SerializeField] int numberOfHealth;
    [SerializeField] Image[] healthBarUnit;
    [SerializeField] Image playerHealthIcon;
    [SerializeField] Sprite spriteHealthFull;
    [SerializeField] Sprite spriteHealthEmpty;

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onPlayerHealthChanged += UpdateHealth;
    }

    void OnDestroy()
    {
        GameEvents.current.onPlayerHealthChanged -= UpdateHealth;
    }

    void GetPlayerStats()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (playerHealth)
        {
            UpdateHealth();
        }
    }

    void UpdateHealth()
    {
        if (playerHealth == null)
        {
            GetPlayerStats();
            return;
        }

        numberOfHealth = (int)playerHealth.healthMax;

        for (int i = 0; i < healthBarUnit.Length; i++)
        {
            //Display current player health
            if (i < playerHealth.health)
            {
                healthBarUnit[i].sprite = spriteHealthFull;
            }
            else healthBarUnit[i].sprite = spriteHealthEmpty;

            //Display max healthbars
            if (i < numberOfHealth)
            {
                healthBarUnit[i].enabled = true;
            }
            else healthBarUnit[i].enabled = false;
        }
    }
}
