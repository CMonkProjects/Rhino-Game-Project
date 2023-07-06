using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Shows the objectives whenever player hits 'tab'
public class Objective_Canvas_Timer : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float eventCanvasTimer;
    [SerializeField] float tabCanvasTimer;

    float currentCanvasTimer;

    bool playerPressedTab;

    void Start()
    {
        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onNewObjective += ResetTimer;
        GameEvents.current.onObjectiveUpdated += ResetTimer;
    }

    void OnDestroy()
    {
        GameEvents.current.onNewObjective -= ResetTimer;
        GameEvents.current.onObjectiveUpdated -= ResetTimer;
    }

    // Update is called once per frame
    void Update()
    {
        currentCanvasTimer -= Time.deltaTime;

        if (Input.GetButton("Tab") && canvasGroup)
        {
            playerPressedTab = true;
            ResetTimer();
        }

        if (currentCanvasTimer > 0f)
        {
            canvasGroup.alpha = 1;
        }
        else canvasGroup.alpha = 0.5f;
    }

    void ResetTimer()
    {
        switch (playerPressedTab)
        {
            case true:
                currentCanvasTimer = tabCanvasTimer;
                playerPressedTab = false;
                break;

            case false:
                currentCanvasTimer = eventCanvasTimer;
                break;
        }
    }
}
