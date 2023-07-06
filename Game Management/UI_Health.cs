using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=5NViMw-ALAo
public class UI_Health : MonoBehaviour
{
    [SerializeField] PlayerStats playerHealth;

    public GameObject healthUnitPrefab;

    public Image imageDebug;

    List<UI_HealthUnit> healthUnitsList = new List<UI_HealthUnit>();

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onPlayerHealthChanged += UpdateHealth;
        GameEvents.current.onPlayerSpawned += GetPlayerStats;
    }

    void OnDestroy()
    {
        GameEvents.current.onPlayerHealthChanged -= UpdateHealth;
        GameEvents.current.onPlayerSpawned -= GetPlayerStats;
    }

    void DrawHealth()
    {
        ClearHealthUnits();

        //Draw out the total number of health units we need based on player's max health

        int healthUnitsToMake = (int)playerHealth.healthMax;

        for (int i = 0; i < healthUnitsToMake; i++)
        {
            CreateEmptyHealthUnit();
        }

        //Now draw our current health
        for (int i = 0; i < healthUnitsList.Count; i++)
        {
            int healthStatus = (int)Mathf.Clamp(playerHealth.health - i, 0, 1);
            healthUnitsList[i].SetHealthUnitImage((HealthStatus)healthStatus);
        }
    }

    void CreateEmptyHealthUnit()
    {
        //Create healthUnit and child it to our transform
        GameObject newHealthUnit = Instantiate(healthUnitPrefab,transform,false);   //We had to do this or else the scale turns out wrong for each health unit

        //Get the component of the new healthUnit, set its image to empty, and add it to the list
        UI_HealthUnit healthUnit = newHealthUnit.GetComponent<UI_HealthUnit>();
        healthUnit.SetHealthUnitImage(HealthStatus.Empty);
        healthUnitsList.Add(healthUnit);
    }

    void ClearHealthUnits()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        healthUnitsList = new List<UI_HealthUnit>();
    }

    void GetPlayerStats()
    {
        //playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerHealth = GameObject.Find("Player Rhino(Clone)").GetComponent<PlayerStats>();

        if (playerHealth)
        {
            DrawHealth();
        }
        else
        {
            if (imageDebug)
                imageDebug.enabled = true;
        }
    }

    void UpdateHealth()
    {
        if (playerHealth)
        {
            DrawHealth();
        }
        else
        {
            GetPlayerStats();
        }
    }
}
